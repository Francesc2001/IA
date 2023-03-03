using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_PROTECTCABBAGE", menuName = "Finite State Machines/FSM_PROTECTCABBAGE", order = 1)]
public class FSM_PROTECTCABBAGE : FiniteStateMachine
{
    private CHICKEN_Blackboard blackboard;

    private SteeringContext steeringContext;

    private Arrive arrive;
    private Seek seek;
    private WanderAround wanderAround;
    private GameObject theAnt;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        blackboard = GetComponent<CHICKEN_Blackboard>();
        steeringContext = GetComponent<SteeringContext>();

        seek = GetComponent<Seek>();
        arrive = GetComponent<Arrive>();
        wanderAround = GetComponent<WanderAround>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */

        base.DisableAllSteerings();

        base.OnExit();
    }

    public override void OnConstruction()
    {
        //finite state machine lo que sea

        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */


        State wanderer = new State("WANDERER",
            () => { wanderAround.attractor = blackboard.attractor; wanderAround.enabled = true; },
            () => { }, //cuidao con esto
            () => { wanderAround.enabled = false; }
            );
         
        

        State angry = new State("ANGRY",
            () => { seek.target = theAnt; seek.enabled = true; steeringContext.maxSpeed *= 2.0f; },
            () => { },
            () => { steeringContext.maxSpeed /= 2.0f; seek.enabled = false; }
            );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition antTooClose = new Transition("ant too close",
            () => { theAnt = SensingUtils.FindInstanceWithinRadius(gameObject, "ANT", blackboard.antDetectionRadius); return theAnt != null; },
            () => { }
            );

        Transition antFarEnough = new Transition("ant far enough",
            () => { return SensingUtils.DistanceToTarget(gameObject, theAnt) > blackboard.antFarEnoughRadius; },
            () => { }
            );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */

        AddStates(wanderer, angry);
        AddTransition(wanderer, antTooClose, angry);
        AddTransition(angry, antFarEnough, wanderer);

        /* STAGE 4: set the initial state
         
        initialState = ... 

         */

        initialState = wanderer; 
    }
}
