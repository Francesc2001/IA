using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_ANTS", menuName = "Finite State Machines/FSM_ANTS", order = 1)]
public class FSM_ANTS : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private WanderAround wanderAround;
    private Flee flee;
    public GameObject chicken;
    public ANTS_Blackboard blackboard;
    public SteeringContext steeringContext;
    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        wanderAround = GetComponent<WanderAround>();
        flee = GetComponent<Flee>();
        chicken = GameObject.FindGameObjectWithTag("CHICKEN");
        blackboard = GetComponent<ANTS_Blackboard>();
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
        base.OnExit();
    }

    public override void OnConstruction()
    {
        // STAGE 1: create the states with their logic(s)
        // *-----------------------------------------------
         
        State goingToCabbage = new State("Going To Cabbage",
            () => { wanderAround.attractor = blackboard.cabbageLocation; wanderAround.enabled = true; steeringContext.seekWeight = 0.2f; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { wanderAround.enabled = false; }  // write on exit logic inisde {}  
        );

        State goingToNest = new State("Going To Nest",
            () => { wanderAround.attractor = blackboard.nestLocation; wanderAround.enabled = true; steeringContext.seekWeight = 0.8f; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { wanderAround.enabled = false; }  // write on exit logic inisde {}  
        );

        State fleeingFromChicken = new State("Fleeing From Chicken",
            () => { flee.target = chicken; flee.enabled = true; steeringContext.maxAcceleration *= 4; steeringContext.maxSpeed *= 9; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { /*flee.enabled = false; */steeringContext.maxAcceleration /= 4; steeringContext.maxSpeed /= 9; }  // write on exit logic inisde {}  
        );



        //* STAGE 2: create the transitions with their logic(s)
        //* ---------------------------------------------------

        Transition cabbageReached = new Transition("Cabbage Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.cabbageLocation) <= blackboard.locationDetectionRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition nestReached = new Transition("Nest Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.nestLocation) <= blackboard.locationDetectionRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition chickenCloseEnough = new Transition("Chicken Close Enough",
            () => { return SensingUtils.DistanceToTarget(gameObject, chicken) <= blackboard.chickenTooClose; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition chickenFarEnough = new Transition("Chicken Far Enough",
            () => { return SensingUtils.DistanceToTarget(gameObject, chicken) >= blackboard.chickenFarAwayRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
        */
        AddStates(goingToCabbage, goingToNest, fleeingFromChicken);
        
        AddTransition(goingToCabbage, chickenCloseEnough, fleeingFromChicken);
        AddTransition(goingToNest, chickenCloseEnough, fleeingFromChicken);
        AddTransition(fleeingFromChicken, chickenFarEnough, goingToCabbage);
        AddTransition(goingToCabbage, cabbageReached, goingToNest);
        AddTransition(goingToNest, nestReached, goingToCabbage);


        initialState = goingToCabbage;
    }
}
