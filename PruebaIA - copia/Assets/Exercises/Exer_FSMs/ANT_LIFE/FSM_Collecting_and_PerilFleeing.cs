using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Collecting_and_PerilFleeing", menuName = "Finite State Machines/FSM_Collecting_and_PerilFleeing", order = 1)]
public class FSM_Collecting_and_PerilFleeing : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private ANT_Blackboard blackboard;
    private Flee flee;
    private GameObject thePredator;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        blackboard = GetComponent<ANT_Blackboard>();
        flee = GetComponent<Flee>();

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
        /* STAGE 1: create the states with their logic(s)  */


        FiniteStateMachine seedCollecting = ScriptableObject.CreateInstance<FSM_SeedCollecting>();

        State fleeingFromPeril = new State("Fleeing Peril",
            () => {
                // Seed being transported, if any, is dropped when seedCollecting is exited
                // so no need to worry about the seed here.
                flee.target = thePredator;
                flee.enabled = true;
            },
            () => { },
            () => { flee.enabled = false; }
        );

        /* STAGE 2: create the transitions with their logic(s) */


        Transition predatorNearby = new Transition("Predator Nearby",
            () => {
                thePredator = SensingUtils.FindInstanceWithinRadius(gameObject, "PREDATOR", blackboard.predatorDetectionRadius);
                return thePredator != null;
            }
        );

        Transition predatorFarAway = new Transition("Predator Far",
            () => { return SensingUtils.DistanceToTarget(gameObject, thePredator) > blackboard.predatorFarAwayRadius; }
        );

        /* STAGE 3: add states and transitions to the FSM  */

        AddStates(seedCollecting, fleeingFromPeril);
        AddTransition(seedCollecting, predatorNearby, fleeingFromPeril);
        AddTransition(fleeingFromPeril, predatorFarAway, seedCollecting);

        /* STAGE 4: set the initial state */

        initialState = seedCollecting;
    }
}
