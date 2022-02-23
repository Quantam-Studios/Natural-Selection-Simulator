using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // MENUS
    // Statistics
    public GameObject statisticsMenu;
    public GameObject mainMenu;
    public GameObject setupMenu;

    // Start is called before the first frame update
    void Start()
    {
        statisticsMenu.SetActive(false);
        mainMenu.SetActive(true);
        setupMenu.SetActive(false);
        Time.timeScale = 0;
    }

    // RUN A NEW SIMULATION 
    // this function is called when the button "Run Simulation Button" is pressed
    // this will close the setup menu, and begin running the simlation with the applied settings
    public void RunSimulation()
    {
        Time.timeScale = 1;
    }
}
