using UnityEngine;
using UnityEngine.UI;

public class GeneralManager : MonoBehaviour
{
    // CREATURE TYPE
    // For interactivity between other scripts
    public static bool sexual;
    public static bool asexual;
    public static bool hermaphrodite;

    // CREATURE SPAWNING
    public GameObject sexualCreatures;
    public GameObject asexualCreatures;
    public GameObject hermaphroditeCreatures;

    // CREATURE STATISTICS
    public GameObject sexualCreatureStats;
    public GameObject asexualCreatureStats;
    public GameObject hermaphroditeCreatureStats;

    private void Start()
    {
        // Initially set all creatures to inactive
        sexualCreatures.SetActive(false);
        asexualCreatures.SetActive(false);
        hermaphroditeCreatures.SetActive(false);
        // Initially set all statistics to inactive;
        sexualCreatureStats.SetActive(false);
        asexualCreatureStats.SetActive(false);
        hermaphroditeCreatureStats.SetActive(false);
    }

    // SETTING OF REPRODUCTIVE TYPE
    // this allows for the user to set the reproductive type inside of the simulation without the editor 
    public void SetReproductiveType(Dropdown dropdown)
    {
        // Initially set all creatures to inactive
        sexualCreatures.SetActive(false);
        asexualCreatures.SetActive(false);
        hermaphroditeCreatures.SetActive(false);
        // Initially set all statistics to inactive
        sexualCreatureStats.SetActive(false);
        asexualCreatureStats.SetActive(false);
        hermaphroditeCreatureStats.SetActive(false);
        // Initiall Set Bools to false
        sexual = false;
        asexual = false;
        hermaphrodite = false;

        // Get the acitve index of the drop down
        int activeReproductionIndex = dropdown.value;

        if (activeReproductionIndex == 0) // sexual
            sexual = true;
        if(activeReproductionIndex == 1) // asexual
            asexual = true;
        if (activeReproductionIndex == 2) // hemaphrodite
            hermaphrodite = true;

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
        // setActive HERMAPHRODITE creature things
        if (hermaphrodite)
        {
            hermaphroditeCreatures.SetActive(true);
            hermaphroditeCreatureStats.SetActive(true);
        }
    }
}
