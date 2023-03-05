using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHICKEN_Blackboard : MonoBehaviour
{
    public float hunger = 0.0f;
    public float tiredness = 0.0f;

    public float maxChasingTime = 15.0f;
    public float maxRestingTime = 3.0f;

    public float eatingTime = 0.5f; 
    public float hungerTooHigh = 100;
    public float hungerLowEnough = 60.0f;
    public float hungerIncrement = 0.5f;
    public float hungerRecovery = 20.0f;

    public float foodReachedRadius = 10f;
    public float foodHasVanishedRadius = 200f;
    public float placeReachedRadius = 15;

    public float tirednessIncrement = 0.5f;
    public float tirednessTooHigh = 100;
    public float tirednessLowEnough = 60.0f;
    public float tirednessRecovery = 20.0f;

    public float antDetectionRadius = 100f;
    public float antFarEnoughRadius = 100f;

    public float foodDetectionRadius = 100f;

    public GameObject henHouse;
    //public GameObject cabbage;
    public GameObject ant;
    public GameObject attractor;


    //maybe añade sonidos

    void Awake()
    {
        attractor = GameObject.Find("Attractor"); //cuidao
    }

    // Start is called before the first frame update
    void Start()
    {
        if (henHouse == null)
        {
            henHouse = GameObject.Find("HOUSE");
            if (henHouse == null)
            {
                Debug.LogError("no HENHOUSE object found in " + this);
            }
        }

        /*if (cabbage == null)
        {
            cabbage = GameObject.Find("CABBAGE");
            if (cabbage == null)
            {
                Debug.LogError("no CABBAGE object found in " + this);
            }
        }*/

        if (ant == null) //maybe no hace falta
        {
            ant = GameObject.Find("ANT");
            if (ant == null)
            {
                Debug.LogError("no ANT object found in " + this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        hunger += hungerIncrement * Time.deltaTime;

        tiredness += tirednessIncrement * Time.deltaTime;
    }
}
