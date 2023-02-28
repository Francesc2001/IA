using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_SeedCollecting", menuName = "Finite State Machines/FSM_SeedCollecting", order = 1)]
public class FSM_SeedCollecting : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private ANT_Blackboard blackboard;
    private Arrive arrive;
    private GameObject theSeed;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        blackboard = GetComponent<ANT_Blackboard>();
        arrive = GetComponent<Arrive>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */

        if (currentState != null && currentState.Name == "TRANSPORTING TO NEST")
        {
            theSeed.transform.parent = null; // drop it
            theSeed.tag = "SEED";
        }

        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */

        FiniteStateMachine twoPointWandering = ScriptableObject.CreateInstance<FSM_TwoPointWandering>();

        State goingToSeed = new State("GOING TO SEED",
            () => {
                arrive.target = theSeed;
                arrive.enabled = true;
            },
            () => { },
            () => { arrive.enabled = false; }
        );

        State transportingToNest = new State("TRANSPORTING TO NEST",
            () => {
                theSeed.tag = "NO_SEED"; // change tag when taken not when dropped
                theSeed.transform.parent = transform;
                arrive.target = blackboard.nest;
                arrive.enabled = true;
            },
            () => { },
            () => {
                arrive.enabled = false;
                theSeed.transform.parent = null;
                theSeed.tag = "NO_SEED"; // no need if retagged when taken
            }
        );

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition seedDetected = new Transition("Seed Detected",
            () => {
                theSeed = SensingUtils.FindInstanceWithinRadius(gameObject, "SEED", blackboard.seedDetectionRadius);
                return theSeed != null;
            }
            // check has done the work
        );

        Transition seedReached = new Transition("Seed Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, theSeed) < blackboard.seedReachedRadius; }
        );

        Transition nestReached = new Transition("Nest Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.nest) < blackboard.nestReachedRadius; }
        );

        // new transition added to avoid ants following "the carrier".
        Transition seedTakenByOther = new Transition("Seed Taken by Other",
            () => { return theSeed.tag != "SEED"; }
        );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */

        AddStates(twoPointWandering, goingToSeed, transportingToNest);

        // addition of new transition
        AddTransition(goingToSeed, seedTakenByOther, twoPointWandering);

        AddTransition(twoPointWandering, seedDetected, goingToSeed);
        AddTransition(goingToSeed, seedReached, transportingToNest);
        AddTransition(transportingToNest, nestReached, twoPointWandering);



        /* STAGE 4: set the initial state
         
        initialState = ... 

         */

        initialState = twoPointWandering;



    }
}
