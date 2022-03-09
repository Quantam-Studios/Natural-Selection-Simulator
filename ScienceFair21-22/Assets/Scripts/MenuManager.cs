using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // MENUS
    // Statistics
    public GameObject statisticsMenu;
    // Main Menu
    public GameObject mainMenu;
    // Setup Menu
    public GameObject setupMenu;
    // Pause Menu
    public GameObject pauseMenu;

    // PAUSING
    public bool paused;
    public bool simStarted;

    // TIME LIMIT
    public Timer timer;

    // CREATURE TYPE
    // For interactivity between other scripts
    public static bool[] activeCreatures = new bool[3];
    public static int activeCreatureIndex;

    // CREATURE SPAWNING
    public GameObject[] creatures;
    public static int initialCreatureCount;
    public CreatureSpawning creatureSpawning;

    // PREDATOR TYPE
    // For interactivity between other scripts
    public static bool[] activePredator = new bool[4];
    public static int activePredatorIndex;

    // PREDATOR SPAWNING
    public GameObject[] predators;
    public static int initialPredatorCount;
    public PredatorSpawning predatorSpawning;
    

    // CREATURE STATISTICS
    public GameObject[] creatureStats;

    // PREDATOR STATISTICS
    public GameObject[] predatorStats;

    // FOOD STUFF
    // Food Manager
    public GameObject foodManager;
    // Initial Food Amount
    public static int initalFood;
    // Food Spawn Rate
    public static float foodSpawnRate;

    // DATA COLLECTION / LOGGING
    // New log sections / setting recording interval
    public CreatureStatistics creatureStatistics;
    // Setting recording interval
    public PredatorStatistics predatorStatistics;
    // general logging
    public static float setDataRecordingInterval;


    // Start is called before the first frame update
    void Start()
    {
        // set menus active or inactive
        statisticsMenu.SetActive(false);
        mainMenu.SetActive(true);
        setupMenu.SetActive(false);
        pauseMenu.SetActive(false);
        // set food manager inactive
        foodManager.SetActive(false);
        // make sure nothing is simulated 
        Time.timeScale = 0;
        // initialize pausing, and simulation state management bools
        paused = false;
        simStarted = false;
    }

    private void Update()
    {
        // PAUSE
        // if "space" key is pressed, the simulation is not paused, and the simulation has started then pause the simulation
        if (Input.GetKeyDown(KeyCode.Space) && !paused && simStarted)
        {
            // set true to make this statement can only run when not paused
            paused = true;
            // stop time
            Time.timeScale = 0;
            // show the pause menu
            pauseMenu.SetActive(true);
        }
    }

    // THE FOLLOWING FUNCTIONS ARE CALLED WHEN BUTTONS ON THE "Pause Menu" ARE PRESSED

    // END SIMULATION
    public void EndSimulation()
    {
        SceneManager.LoadScene(0);
    }

    // RESUME SIMULATION
    public void ResumeSimulation()
    {
        // stop showing the pause menu
        pauseMenu.SetActive(false);
        // set false to allow pausing to happen again
        paused = false;
        // start time
        Time.timeScale = 1;
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
        simStarted = true;
        Time.timeScale = 1;
        // begin logging
        creatureStatistics.initializeNewLog();
        predatorStatistics.SetRecordingInterval();
    }

    // SET CREATURE
    public void SetCreature(Dropdown creatureType)
    {
        // set creature type active
        creatures[creatureType.value].SetActive(true);
        activeCreatureIndex = creatureType.value;
        statisticsMenu.SetActive(true);
        // set creature statistics active
        creatureStats[creatureType.value].SetActive(true);
        activeCreatures[creatureType.value] = true;
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
            // set predator statistics active
            predatorStats[predatorType.value].SetActive(true);
        }
        // set activePredatorIndex for other scripts to act upon
        activePredatorIndex = predatorType.value;
    }

    // SET TIME LIMIT
    public void SetTimeLimit(Dropdown timeLimit)
    {
        // set time limit
        timer.activeTimeLimitIndex = timeLimit.value;
    }

    // SET INITIAL FOOD COUNT
    public void SetInitialFood(InputField foodCount)
    {
        // set the inital food count to the value of foodCount if the text is not effectively null
        if(foodCount.text != "")
            initalFood = Convert.ToInt32(foodCount.text);
    }

    // SET FOOD SPAWN RATE
    public void SetFoodSpawnRate(InputField spawnRate)
    {
        // set the food spawn rate to value of spawnRate if the text is not effectively null
        if (spawnRate.text != "")
            foodSpawnRate = float.Parse(spawnRate.text, System.Globalization.CultureInfo.InvariantCulture);
        else // if not set then defualt to 1 second
            foodSpawnRate = 1;
    }

    // SET DATA COLLECTION INTERVAL
    public void SetDataCollectionRate(InputField collectionRate)
    {
        // set the collection rate to value of collectionRate if the text is not effectively null
        if (collectionRate.text != "")
            setDataRecordingInterval = float.Parse(collectionRate.text, System.Globalization.CultureInfo.InvariantCulture);
        else // the value was not set so defualt to 2 seconds
            setDataRecordingInterval = 2f;           
    }

    // SET START AMOUNT OF CREATURES (prey)
    public void SetInitialCreatureCount(InputField initialAmount)
    {
        // set the inital creature count to value of initialAmount if the text is not effectively null
        if (initialAmount.text != "")
            initialCreatureCount = Convert.ToInt16(initialAmount.text, System.Globalization.CultureInfo.InvariantCulture);
        else // default to four creatures (prey)
            initialCreatureCount = 4;
        // start spawning
        SpawnInitialCreatures();       
    }

    // START SPAWNING OF PREY
    void SpawnInitialCreatures()
    {
        // start spawning of inital creatures
        creatureSpawning.spawnInitialCreatures(initialCreatureCount, activeCreatureIndex);
    }

    // SET START AMOUNT OF PREDATORS
    public void SetInitalPredatorCount(InputField initialAmount)
    {
        // set the inital predator count to value of initialAmount if the text is not effectively null
        if (initialAmount.text != "")
            initialPredatorCount = Convert.ToInt16(initialAmount.text, System.Globalization.CultureInfo.InvariantCulture);
        else // default to four predators
            initialPredatorCount = 4;
        if (activePredatorIndex != 3)
        {
            // start spawning
            SpawnInitalPredators();
        }
    }

    // START SPAWNING OF PREDATORS
    void SpawnInitalPredators()
    {
        // start spawning of inital predators
        predatorSpawning.spawnInitialPredators(initialPredatorCount, activePredatorIndex);
    }
}
