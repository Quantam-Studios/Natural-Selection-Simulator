using UnityEngine;
using UnityEngine.UI;

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
    public static bool[] activePredator = new bool[3];

    // PREDATOR SPAWNING
    public GameObject[] predators;

    // CREATURE STATISTICS
    public GameObject[] creatureStats;


    // Start is called before the first frame update
    void Start()
    {
        // set menus active or inactive
        statisticsMenu.SetActive(false);
        mainMenu.SetActive(true);
        setupMenu.SetActive(false);
    }

    // RUN A NEW SIMULATION 
    // this function is called when the button "Run Simulation Button" is pressed
    // this will close the setup menu, and begin running the simlation with the applied settings
    public void RunSimulation()
    {
        // set statistics menu active
        statisticsMenu.SetActive(true);
        Time.timeScale = 1;
    }

    // SET CREATURE
    // this function is called when the button "Run Simulation Button" is pressed
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
    // this function is called when the button "Run Simulation Button" is pressed
    public void SetPredator(Dropdown predatorType)
    {
        // set predator type active
        predators[predatorType.value].SetActive(true);
        activePredator[predatorType.value] = true;
    }
}
