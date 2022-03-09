using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorSpawning : MonoBehaviour
{
    // PREDATORS
    [Header("Predators")]
    public GameObject[] predators;
    // predator holders
    public Transform[] predatorHolders;

    // BOUNDS
    [Header("Boundaries")]
    // min and max (x,y) values for food spawn locations
    public float maxX;
    public float maxY;
    public float minX;
    public float minY;

    // SPAWNER
    // this function is called from the SpawnInitialPredators() function in MenuManager which is called when the button "Run Simulation Button" is pressed
    public void spawnInitialPredators(int predatorCount, int activePredator)
    {
        for (int i = 0; i < predatorCount; i++)
        {
            // generate random position
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            Vector3 spawnPos = new Vector3(x, y, 1);

            // spawn the predator
            // if the predator is NOT sexual
            if (activePredator != 0)
                Instantiate(predators[activePredator], spawnPos, Quaternion.identity, predatorHolders[activePredator]);
            else // else if the predator is sexual
            {
                // Ensures a close to even if not perfectly even distribution of females, and males.
                if(i % 2 != 0) // spawn a female
                    Instantiate(predators[0], spawnPos, Quaternion.identity, predatorHolders[activePredator]);
                else // spawn a male
                    Instantiate(predators[3], spawnPos, Quaternion.identity, predatorHolders[activePredator]);
            }
        }
    }
}
