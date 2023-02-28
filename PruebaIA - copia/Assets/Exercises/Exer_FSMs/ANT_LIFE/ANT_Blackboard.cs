
using UnityEngine;

public class ANT_Blackboard : MonoBehaviour
{
    //[Header("Two point wandering")]

    public GameObject locationA;
    public GameObject locationB;
    public float intervalBetweenTimeOuts = 10f;
    public float initialSeekWeight = 0.2f;
    public float seekIncrement = 0.2f;
    public float locationReachedRadius = 10f;

    //[Header("Seed colecting")]

    public GameObject nest;
    public float seedDetectionRadius;
    public float seedReachedRadius;
    public float nestReachedRadius;


    //[Header("Peril Fleeing")]


    void Start()
    {
       
    }

   
}
