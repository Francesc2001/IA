using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_TwoPointWandering", menuName = "Finite State Machines/FSM_TwoPointWandering", order = 1)]
public class FSM_TwoPointWandering : FiniteStateMachine
{
    

    private WanderAround wanderAround;
    private SteeringContext steeringContext;
    private ANT_Blackboard blackboard;

    private float elapsedTime = 0;


    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is executed every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<ANT_Blackboard>();
        /* COMPLETE */
        wanderAround = GetComponent<WanderAround>();
        steeringContext = GetComponent<SteeringContext>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */

        /* COMPLETE */
        steeringContext.seekWeight = blackboard.initialSeekWeight;
        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         */

        State goingA = new State("Going_A",
           () => { wanderAround.attractor = blackboard.locationA; elapsedTime = 0; wanderAround.enabled = true; },
           () => { elapsedTime += Time.deltaTime;}, 
           () => { wanderAround.enabled = false; }
       );

        State goingB = new State("Going_B",
           () => { wanderAround.attractor = blackboard.locationB; elapsedTime = 0; wanderAround.enabled = true; },
           () => { elapsedTime += Time.deltaTime; },
           () => { wanderAround.enabled = false; }
       );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------
        */

        Transition locationAreached = new Transition("Location A reached",
            () =>
            {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.locationA) < blackboard.locationReachedRadius;
            },
            () => { steeringContext.seekWeight = blackboard.initialSeekWeight; }
            );

        Transition locationBreached = new Transition("Location B reached",
            () =>
            {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.locationB) < blackboard.locationReachedRadius;
                
            },
            () => { steeringContext.seekWeight = blackboard.initialSeekWeight; }
            );


        /*Transition timeOut = new Transition("TimeOut",
            () =>
            {
                bool to = elapsedTime

            },
            () => { steeringContext.seekWeight = blackboard.initialSeekWeight; }
            );
        /*
        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        */

        /* COMPLETE, create the transitions */

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
         */

        AddStates(goingA, goingB);

        /* COMPLETE, add the transitions */

        AddTransition(goingA, locationAreached, goingB);
        AddTransition(goingB, locationBreached, goingA);

        /* STAGE 4: set the initial state */

        initialState = goingA;
    }
}
