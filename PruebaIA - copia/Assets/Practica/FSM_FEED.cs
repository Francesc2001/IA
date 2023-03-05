using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_FEED", menuName = "Finite State Machines/FSM_FEED", order = 1)]
public class FSM_FEED : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private CHICKEN_Blackboard blackboard;

    private SteeringContext steeringContext; // maybe no es necesario

    private Arrive arrive;

    private GameObject food; //no se si la comida son las hormigas o que

    private Wander wander; 

    private float elapsedTime; 

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
       

        blackboard = GetComponent<CHICKEN_Blackboard>();

        steeringContext = GetComponent<SteeringContext>();

        //continuar

        arrive = GetComponent<Arrive>();

        wander = GetComponent<Wander>(); 


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
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */

        State lookingForFood = new State("LOOKING FOR FOOD",

            () => {wander.enabled = true; }, //
            () => { },
            () => { wander.enabled = false; }  //
            );

        FiniteStateMachine notHungry = ScriptableObject.CreateInstance<FSM_PROTECTCABBAGE>();

        State goingToFood = new State("GOING TO FOOD",

            () => {arrive.target = food; arrive.enabled = true; },
            () => { },
            () => { arrive.enabled = false; }

            );

        State eating = new State("EATING",

            () => { elapsedTime = 0.0f; },
            () => { elapsedTime += Time.deltaTime; },
            () => { blackboard.hunger -= blackboard.hungerRecovery; Destroy(food); }

            );

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */


        Transition detectedFood = new Transition("DETECTED FOOD",
            () => { food = SensingUtils.FindInstanceWithinRadius(gameObject, "WORM", blackboard.foodDetectionRadius); return food != null; },
            () => { }
            );

        Transition arriveToFood = new Transition("ARRIVE TO FOOD",

            () => { return SensingUtils.DistanceToTarget(gameObject, food) < blackboard.foodReachedRadius; },
            () => { }

            );

        Transition foodEaten = new Transition("FOOD EATEN",

            () => { return elapsedTime >= blackboard.eatingTime; },
            () => { }

            );

        Transition hungry = new Transition("HUNGRY",

            () => { return blackboard.hunger >= blackboard.hungerLowEnough;  },
            () => { }

            );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        
        AddStates(lookingForFood, goingToFood, eating, notHungry); //añadir estado
        AddTransition(lookingForFood, detectedFood, goingToFood);
        AddTransition(goingToFood, arriveToFood, eating);
        AddTransition(eating, foodEaten, notHungry);
        AddTransition(notHungry, hungry, lookingForFood);

        /* STAGE 4: set the initial state
         
        initialState = ... 

         */

        initialState = notHungry;

    }
}
