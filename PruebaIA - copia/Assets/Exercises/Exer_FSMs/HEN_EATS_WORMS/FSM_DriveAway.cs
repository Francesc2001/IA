using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_DriveAway", menuName = "Finite State Machines/FSM_DriveAway", order = 1)]
public class FSM_DriveAway : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private Seek seek;
    private HEN_Blackboard blackboard;
    private AudioSource audioSource;
    private SteeringContext steeringContext;
    private GameObject chick;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        seek = GetComponent<Seek>();
        blackboard = GetComponent<HEN_Blackboard>();
        audioSource = GetComponent<AudioSource>();
        steeringContext = GetComponent<SteeringContext>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        audioSource.Stop();
        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        // STAGE 1: create the states with their logic(s)
        //-----------------------------------------------

        FiniteStateMachine search = ScriptableObject.CreateInstance<FSM_SearchWorms>();
        search.Name = "Search";

        State driveAwayChicks = new State("Drive Away Chicks",
            () => { steeringContext.transform.localScale *= 1.4f; steeringContext.maxAcceleration *= 2; steeringContext.maxSpeed += 1;
                seek.target = chick; seek.enabled = true; audioSource.clip = blackboard.angrySound; audioSource.Play(); }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { steeringContext.transform.localScale /= 1.4f; steeringContext.maxAcceleration /= 2; steeringContext.maxSpeed -= 1;
                seek.enabled = false; audioSource.Stop(); }  // write on exit logic inisde {}  
        );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition chickTooClose = new Transition("Chick Too Close",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        /*
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
