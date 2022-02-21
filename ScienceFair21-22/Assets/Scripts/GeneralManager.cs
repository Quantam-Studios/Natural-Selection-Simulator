using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    // CREATURE TYPE
    [Header("Select a SINGLE Creature Type")]
    public bool setSexual;
    public bool setAsexual;
    public static bool sexual;
    public static bool asexual;

    // CREATURE SPAWNING
    public GameObject sexualCreatures;
    public GameObject asexualCreatures;

    // CREATURE STATISTICS
    public GameObject sexualCreatureStats;
    public GameObject asexualCreatureStats;


    private void Start()
    {
        // Set which creatures to spawn
        asexual = setAsexual;
        sexual = setSexual;
        // Initially set all creatures to inactive
        sexualCreatures.SetActive(false);
        asexualCreatures.SetActive(false);
        // Initially set all statistics to inactive;
        sexualCreatureStats.SetActive(false);
        asexualCreatureStats.SetActive(false);

        // SET ACTIVE THE SELECTED CREATURE'S THINGS
        // setActive SEXUAL creature things
        if (sexual)
        {
            sexualCreatures.SetActive(true);
            sexualCreatureStats.SetActive(true);
        }
        // setActive ASEXUAL creature things
        if (asexual)
        {
            asexualCreatures.SetActive(true);
            asexualCreatureStats.SetActive(true);
        }
    }
}
