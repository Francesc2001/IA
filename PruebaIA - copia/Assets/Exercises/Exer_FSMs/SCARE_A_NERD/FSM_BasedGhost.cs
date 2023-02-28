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
         
        State goCastle = new State("Go Castle",
            () => { arrive.target = blackboard.castle; arrive.enabled = true; currentSpeed = maxSpeed; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { arrive.enabled = false; currentSpeed = normalSpeed; }  // write on exit logic inisde {}  
        );

        State hide = new State("Hide",
            () => { elapsedTime = 0f; }, // write on enter logic inside {}
            () => { elapsedTime += Time.deltaTime; }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );


        State selectTarget = new State("Select Target",
            () => { }, // write on enter logic inside {}
            () => { SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.victimLabel, blackboard.nerdDetectionRadius); }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );


        /*
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */

    }
}
