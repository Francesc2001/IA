using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "newFSM", menuName = "Finite State Machines/newFSM", order = 1)]
public class FSM_RestlessHen : FiniteStateMachine
{
    /*private HEN_Blackboard blackboard;
    private SteeringContext steeringContext;
    private float normalWeight;
    private Color normalColor;

    public override void OnEnter()
    {
         /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations *

        blackboard = GetComponent<HEN_Blackboard>();
        steeringContext = GetComponent<SteeringContext>();
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. *
        base.DisableAllSteerings();
        base.OnExit();
    }
    */
    public override void OnConstruction()
    {
        /*
        FiniteStateMachine eatAlone = ScriptableObject.CreateInstance<FSM_DriveAway>();

        State gettingCloser = new State("GETTING CLOSER",
            () => {
                SpriteRenderer spr = GetComponent<SpriteRenderer>();
                normalColor = spr.color;
                spr.color = blackboard.restlessColor;
                normalWeight = steeringContext.seekWeight;
                steeringContext.seekWeight = 0.7f;
                GetComponent<WanderAround>().enabled = true;
            },
            () => { },
            () => {
                steeringContext.seekWeight = normalWeight;
                GetComponent<WanderAround>().enabled = false;
                GetComponent<SpriteRenderer>().color = normalColor;
            }
        );

        Transition tooFarFromAttractor = new Transition("Too Far From Attractor",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.attractor) >= blackboard.tooFarFromAttractor; }
        );

        Transition closeEnoughToAttractor = new Transition("Close Enough to Attractor",
           () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.attractor) < blackboard.closeEnoughToAttractor; }
        );


        AddStates(eatAlone, gettingCloser);
        AddTransition(eatAlone, tooFarFromAttractor, gettingCloser);
        AddTransition(gettingCloser, closeEnoughToAttractor, eatAlone);

        initialState = eatAlone;
        */
    }
}
