using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_REST", menuName = "Finite State Machines/FSM_REST", order = 1)]
public class FSM_REST : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private CHICKEN_Blackboard blackboard;

    private float restingTime;


    private SteeringContext steeringContext;


    private Arrive arrive;

    private float elapsedTime; 

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        blackboard = GetComponent<CHICKEN_Blackboard>();

        steeringContext = GetComponent<SteeringContext>();

        arrive = GetComponent<Arrive>();

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
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */

        FiniteStateMachine notTired = ScriptableObject.CreateInstance<FSM_FEED>();

        State goToHouse = new State("ARRIVE TO HOUSE",

            () => { arrive.target = blackboard.henHouse; arrive.enabled = true; },
            () => { },
            () => { arrive.enabled = false; }

            );

        State resting = new State("RESTING",

            () => { elapsedTime = 0.0f; },
            () => { elapsedTime += Time.deltaTime; },
            () => { blackboard.tiredness -= blackboard.tirednessRecovery; }

            );

        

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition arriveToHouse = new Transition("ARRIVE TO HOUSE",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.henHouse) <= blackboard.placeReachedRadius; }, //cuidao
            () => { }
            );

        Transition rested = new Transition("RESTED",
            () => { return elapsedTime >= blackboard.maxRestingTime; }, 
            () => { }
            );

        Transition tired = new Transition("TIRED",

            () => { return blackboard.tiredness >= blackboard.tirednessLowEnough; },
            () => { }

            );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */

        AddStates(notTired, goToHouse, resting);
        AddTransition(goToHouse, arriveToHouse, resting);
        AddTransition(resting, rested, notTired);
        AddTransition(notTired, tired, goToHouse);

        /* STAGE 4: set the initial state
         
        initialState = ... 

         */

        initialState = notTired;

    }
}
