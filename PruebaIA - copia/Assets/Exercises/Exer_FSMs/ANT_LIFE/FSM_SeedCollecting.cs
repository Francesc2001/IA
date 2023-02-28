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
    private GameObject seed;

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
        if (currentState != null && currentState.Name == "Transport To Nest")
        {
            seed.transform.parent = null; // drop it
            seed.tag = "SEED";
        }
        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        // STAGE 1: create the states with their logic(s)
        //-----------------------------------------------

        FiniteStateMachine twoPointWandering = ScriptableObject.CreateInstance<FSM_TwoPointWandering>();

         
        State goingToSeed = new State("Going To Seed",
            () => { arrive.target = seed; arrive.enabled = true; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { arrive.enabled = false; }  // write on exit logic inisde {}  
        );


        State transportToNest = new State("Transport To Nest",
            () => { seed.transform.parent = transform; arrive.target = blackboard.nest; arrive.enabled = true; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { arrive.enabled = false; seed.transform.parent = null; seed.tag = "NO SEED"; }  // write on exit logic inisde {}  
        );



        // STAGE 2: create the transitions with their logic(s)
        // ---------------------------------------------------

        Transition seedDetected = new Transition("Seed Detected",
            () => {
                seed = SensingUtils.FindInstanceWithinRadius(gameObject, "SEED", blackboard.seedDetectionRadius);
                return seed != null;
            }
            // check has done the work
        );

        Transition seedReached = new Transition("Seed Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, seed) < blackboard.seedReachedRadius; }
        );

        Transition nestReached = new Transition("Nest Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.nest) < blackboard.nestReachedRadius; }
        );

        Transition seedTakenByOther = new Transition("Seed Taken by Other",
            () => { return seed.tag != "SEED"; }
        );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */

        AddStates(twoPointWandering, goingToSeed, transportToNest);

        // addition of new transition
        AddTransition(goingToSeed, seedTakenByOther, twoPointWandering);

        AddTransition(twoPointWandering, seedDetected, goingToSeed);
        AddTransition(goingToSeed, seedReached, transportToNest);
        AddTransition(transportToNest, nestReached, twoPointWandering);

        //* STAGE 4: set the initial state

        initialState = twoPointWandering;

        

    }
}
