using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_BasedGhost", menuName = "Finite State Machines/FSM_BasedGhost", order = 1)]
public class FSM_BasedGhost : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private GHOST_Blackboard blackboard;
    private Arrive arrive;
    private GameObject target;
    private AudioSource audioSource;
    private Pursue pursue;
    private SteeringContext steeringContext;
    private float elapsedTime;
    private float normalSpeed;
    private float maxSpeed;
    private float currentSpeed;


    public void Awake()
    {
        normalSpeed = steeringContext.maxSpeed;
        maxSpeed = steeringContext.maxSpeed * 4;
    }

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        arrive = GetComponent<Arrive>();
        audioSource = GetComponent<AudioSource>();
        pursue = GetComponent<Pursue>();
        blackboard = GetComponent<GHOST_Blackboard>();
        steeringContext = GetComponent<SteeringContext>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        DisableAllSteerings();
        audioSource.Stop();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        // STAGE 1: create the states with their logic(s)
        //-----------------------------------------------

        State goCastle = new State("GO CASTLE",
            () => {
                steeringContext.maxSpeed *= 4;
                arrive.target = blackboard.castle;
                arrive.enabled = true;
            },
            () => { },
            () => {
                steeringContext.maxSpeed /= 4;
                arrive.enabled = false;
            }
        );

        State hide = new State("HIDE",
            () => { elapsedTime = 0f; },
            () => { elapsedTime += Time.deltaTime; },
            () => { }
        );

        State selectTarget = new State("SELECT TARGET",
            () => { /* DO NOTHING */},
            () => { target = SensingUtils.FindRandomInstanceWithinRadius(gameObject, blackboard.victimLabel, blackboard.nerdDetectionRadius); },
            () => { }
        );

        State approach = new State("APPROACH",
            () => {
                pursue.target = target;
                pursue.enabled = true;
            },
            () => { },
            () => { /* we'll let pursue go active... */}
        );

        State cryBoo = new State("CRY BOO",
            () => { elapsedTime = 0; blackboard.CryBoo(true); },
            () => { elapsedTime += Time.deltaTime; },
            () => {
                blackboard.CryBoo(false);
                pursue.enabled = false;
            }
        );


        Transition castleReached = new Transition("CASTLE REACHED",
             () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.castle) < blackboard.castleReachedRadius; }
        );

        Transition timeOutOne = new Transition("TIMEOUT_ONE",
             () => { return elapsedTime >= blackboard.hideTime; }
        );

        Transition timeOutTwo = new Transition("TIMEOUT_ONE",
             () => { return elapsedTime >= blackboard.booDuration; }
        );

        Transition targetSelected = new Transition("TARGET SELECTED",
             () => { return target != null; }
        );

        Transition targetIsClose = new Transition("TARGET IS CLOSE",
             () => { return SensingUtils.DistanceToTarget(gameObject, target) < blackboard.closeEnoughToScare; }
        );

        AddStates(goCastle, hide, selectTarget, approach, cryBoo);
        AddTransition(goCastle, castleReached, hide);
        AddTransition(hide, timeOutOne, selectTarget);
        AddTransition(selectTarget, targetSelected, approach);
        AddTransition(approach, targetIsClose, cryBoo);
        AddTransition(cryBoo, timeOutTwo, goCastle);

        initialState = goCastle;

    }
}
