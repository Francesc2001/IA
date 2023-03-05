using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWorms : MonoBehaviour
{
    private GameObject sample;

    public float interval = 10f; // one seed every interval seconds
    public float maxX = 100;
    public float maxY = 100;

    private float elapsedTime = 0f; // time elapsed since last generation

    void Start()
    {
        sample = Resources.Load<GameObject>("WORM");
        if (sample == null)
            Debug.LogError("No SEED prefab found as a resource");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject clone;
        if (elapsedTime >= interval)
        {
            // spawn creating an instance...
            clone = Instantiate(sample);
            clone.transform.position = new Vector3(maxX * Steerings.Utils.binomial(), maxY * Steerings.Utils.binomial(), 0);

            elapsedTime = 0;
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }

    }
}
