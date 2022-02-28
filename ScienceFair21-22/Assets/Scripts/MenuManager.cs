using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuManager : MonoBehaviour
{
    // MENUS
    // Statistics
    public GameObject statisticsMenu;
    public GameObject mainMenu;
    public GameObject setupMenu;

    // CREATURE TYPE
    // For interactivity between other scripts
    public static bool[] activeCreature = new bool[3];

    // CREATURE SPAWNING
    public GameObject[] creatures;

    // PREDATOR TYPE
    // For interactivity between other scripts
    public static bool[] activePredator = new bool[4];

    // PREDATOR SPAWNING
    public GameObject[] predators;

    // CREATURE STATISTICS
    public GameObject[] creatureStats;

    // FOOD STUFF
    // Food Manager
    public GameObject foodManager;
    // Initial Food Amount
    public static int initalFood;
    // Food SPawn Rate
    public static float foodSpawnRate;

    // Start is called before the first frame update
    void Start()
    {
        // set menus active or inactive
        statisticsMenu.SetActive(false);
        mainMenu.SetActive(true);
        setupMenu.SetActive(false);
        // set food manager inactive
        foodManager.SetActive(false);
        // make sure nothing is simulated 
        Time.timeScale = 0;
    }

    // THE FOLLOWING FUNCTIONS ARE CALLED WHEN THE BUTTON "Run Simulation Button" IS PRESSED

    // RUN A NEW SIMULATION 
    // this will close the setup menu, and begin running the simlation with the applied settings
    public void RunSimulation()
    {
        // set statistics menu active
        statisticsMenu.SetActive(true);
        // set food spawner active
        foodManager.SetActive(true);
        // begin simulating
        Time.timeScale = 1;
    }

    // SET CREATURE
    public void SetCreature(Dropdown creatureType)
    {
        // set creature type active
        creatures[creatureType.value].SetActive(true);
        statisticsMenu.SetActive(true);
        // set creature statistics active
        creatureStats[creatureType.value].SetActive(true);
        activeCreature[creatureType.value] = true;
    }

    // SET PREDATOR
    public void SetPredator(Dropdown predatorType)
    {
        // check if predatorType value is not 3 (none) 
        if(predatorType.value != 3)
        {
            // set predator type active
            predators[predatorType.value].SetActive(true);
            activePredator[predatorType.value] = true;
        }
    }

    // SET INITIAL FOOD COUNT
    public void SetInitialFood(InputField foodCount)
    {
        // set the inital food count to the value of foodCount if the text is not effectively null
        if(foodCount.text != "")
            initalFood = Convert.ToInt32(foodCount.text.ToString());
    }

    // SET FOOD Spawn Rate
    public void SetFoodSpawnRate(InputField spawnRate)
    {
        // set the food spawn rate to value of spawnRate if the text is not effectively null
        if (spawnRate.text != "")
            foodSpawnRate = float.Parse(spawnRate.text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
    }

}
