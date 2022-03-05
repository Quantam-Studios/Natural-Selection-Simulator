using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawning : MonoBehaviour
{
    // CREATURES
    [Header("Creatures")]
    public GameObject[] creatures;
    // creature holders
    public Transform[] creatureHolders;

    // BOUNDS
    [Header("Boundaries")]
    // min and max (x,y) values for food spawn locations
    public float maxX;
    public float maxY;
    public float minX;
    public float minY;

    // SPAWNER
    // this function is called from the SpawnInitialCreatures() function in MenuManager which is called when the button "Run Simulation Button" is pressed
    public void spawnInitialCreatures(int creatureCount, int activeCreature)
    {
        for (int i = 0; i < creatureCount; i++)
        {
            // generate random position
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            Vector3 spawnPos = new Vector3(x, y, 1);

            // spawn the creature
            // if the creature is NOT sexualCreature
            if (activeCreature != 0)
                Instantiate(creatures[activeCreature], spawnPos, Quaternion.identity, creatureHolders[activeCreature]);
            else // else if the creature is sexualCreature
            {
                // choose random sex
                int randSex = Random.Range(0,2);

                if(randSex == 0) // spawn a female
                    Instantiate(creatures[0], spawnPos, Quaternion.identity, creatureHolders[activeCreature]);
                else // spawn a male
                    Instantiate(creatures[3], spawnPos, Quaternion.identity, creatureHolders[activeCreature]);
            }
        }
    }
}
