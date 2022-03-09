using UnityEngine;
using UnityEngine.UI;
using System;

public class CreatureStatistics : MonoBehaviour
{
    // STATIC VARIABLES FOR TRACKING
    // Seuxal Creature Counts
    public static int femaleSexualCreatureCount;
    public static int maleSexualCreatureCount;
    public static int sexualCreatureCount;
    public static int allTimeFemaleCreatureCount;
    public static int allTimeMaleCreatureCount;
    public static int allTimeSexualCreatureCount;
    // Aseuxal Creature Counts
    public static int asexualCreatureCount;
    public static int allTimeAsexualCreatureCount;
    // Hermaphrodite Creature Counts
    public static int hermaphroditeCreatureCount;
    public static int allTimeHermaphroditeCreatureCount;

    // TEXT OBJECTS FOR DISPLAYING STATISTICS
    // Sexual
    [Header("Sexual Prey Stat Text")]
    public Text sexualCreatureCountText;
    public Text maleSexualCreatureCountText;
    public Text femaleSexualCreatureCountText;
    public Text allTimeSexualCreatureCountText;
    public Text allTimeMaleSexualCreatureCountText;
    public Text allTimeFemaleSexualCreatureCountText;
    // Asexual
    [Header("Asexual Prey Stat Text")]
    public Text asexualCreatureCountText;
    public Text allTimeAsexualCreatureCountText;
    // Hermaphrodite
    [Header("Hermaphrodite Prey Stat Text")]
    public Text hermaphroditeCreatureCountText;
    public Text allTimeHermaphroditeCreatureCountText;

    // COLLECTING DATA
    [Header("Collecting Data")]
    public float setRecordInterval;
    // run time effected countdown
    private float recordInterval;
    // reference to the SaveData script allowing for writing to logs
    public SaveData saveData;
    // reference to the Timer script allowing for access to it's values
    public Timer timer;
    [Header("Formatting Data")]
    // this stuff is for formatting of data in the file "Log.txt"
    public string[] activeCreatureName;
    public string[] activePredatorName;
   
    private void Start()
    {
        // Make the simlation run at a stable frame rate
        Application.targetFrameRate = 60;
        // RESET ALL VALUES
        // this ensures that when the functions used to "reload" the scene static statistic values start at 0
        // Reset all time stats
        allTimeAsexualCreatureCount = 0;
        allTimeSexualCreatureCount = 0;
        allTimeFemaleCreatureCount = 0;
        allTimeMaleCreatureCount = 0;
        allTimeHermaphroditeCreatureCount = 0;
        // Reset current stats
        asexualCreatureCount = 0;
        sexualCreatureCount = 0;
        maleSexualCreatureCount = 0;
        femaleSexualCreatureCount = 0;
        hermaphroditeCreatureCount = 0;
    }

    // INITIALIZE / FORMAT NEW SET OF DATA
    // only called in the RunSimulation() function of MenuManager
    // this ensures that all values, and settings chosen by the user have been set. 
    public void initializeNewLog()
    {
        // Set recordingInterval 
        setRecordInterval = MenuManager.setDataRecordingInterval;
        // Create a description of the circumstances on the log
        saveData.CreateText("\n\n New Data Set \n" + activeCreatureName[MenuManager.activeCreatureIndex] + " creatures with " + activePredatorName[MenuManager.activePredatorIndex] + " predators\n");
        // Determine and format time limit into a string
        TimeSpan timeSpan = TimeSpan.FromSeconds(0 + timer.timeLimits[timer.activeTimeLimitIndex]);
        string timeLimit = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        // Add all presets, and inital values to the log
        saveData.CreateText("Time Limit: " + timeLimit + " Inital Creature Count: " + MenuManager.initialCreatureCount.ToString() + " Initial Food Count: " + MenuManager.initalFood + " Food Spawn Rate: " + MenuManager.foodSpawnRate + "\n");
    }

    // Update is called once per frame
    void Update()
    {
        // Update relevant statistics of PREY
        if (MenuManager.activeCreatures[0])
            SexualCreatureTextUpdate();
        if (MenuManager.activeCreatures[1])
            AsexualCreatureTextUpdate();
        if (MenuManager.activeCreatures[2])
            HermaphroditeCreatureTextUpdate();

        // DATA COLLECTION
        // countdown from the set recording interval
        recordInterval -= Time.deltaTime;
        // once the countdown reaches zero record the proper data, and reset the timer
        if (recordInterval <= 0)
        {
            // restart countdown
            recordInterval = setRecordInterval;
            // log relevant statistics
            if (MenuManager.activeCreatures[0])
                LogSexual();
            if (MenuManager.activeCreatures[1])
                LogAsexual();
            if (MenuManager.activeCreatures[2])
                LogHermaphrodite();
        }
    }

    // LOGGING FUNCTIONS
    // the following functions tell saveData what new content to add to the file "Log.txt"
    // Asexual
    void LogAsexual()
    {
        saveData.CreateText("Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Creatures: " + asexualCreatureCount.ToString() + " All Time Creatures: " + allTimeAsexualCreatureCount.ToString());
    }
    // Sexual
    void LogSexual()
    {
        saveData.CreateText("Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Creatures: " + sexualCreatureCount.ToString() + " All Time Creatures: " + allTimeSexualCreatureCount.ToString());
    }
    // Hermaphrodite
    void LogHermaphrodite()
    {
        saveData.CreateText("Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Creatures: " + hermaphroditeCreatureCount.ToString() + " All Time Creatures: " + allTimeHermaphroditeCreatureCount.ToString());
    }

    // Update sexual statistics text
    void SexualCreatureTextUpdate()
    {
        // SEXUAL STATS
        // Update the total sexual creature count text
        sexualCreatureCountText.text = "Total Sexual Creatures: " + sexualCreatureCount.ToString();
        // Update the total male sexual creature count text
        maleSexualCreatureCountText.text = "Total Male Creatures: " + maleSexualCreatureCount.ToString();
        // Update the total female sexual creature count text
        femaleSexualCreatureCountText.text = "Total Female Creatures: " + femaleSexualCreatureCount.ToString();
        // Update the all time total sexual creature count text
        allTimeSexualCreatureCountText.text = "Total Sexual Creatures Born: " + allTimeSexualCreatureCount.ToString();
        // Update the all time total sexual creature count text
        allTimeMaleSexualCreatureCountText.text = "Total Male Creatures Born: " + allTimeMaleCreatureCount.ToString();
        // Update the all time total female sexual creature count text
        allTimeFemaleSexualCreatureCountText.text = "Total Female Creatures Born: " + allTimeFemaleCreatureCount.ToString();
    }

    // Update asexual statistics text
    void AsexualCreatureTextUpdate()
    {
        // ASEXUAL STATS
        // Update the total asexual creature count text
        asexualCreatureCountText.text = "Total Asexual Creatures: " + asexualCreatureCount.ToString();
        // Update the all time total asexual creature count text
        allTimeAsexualCreatureCountText.text = "Total Asexual Creatures Born: " + allTimeAsexualCreatureCount.ToString();
    }

    // Update hermaphrodite statistics text
    void HermaphroditeCreatureTextUpdate()
    {
        // HERMAPHRODITE STATS
        // Update the total hermaphrodite creature count text
        hermaphroditeCreatureCountText.text = "Total Hermpahrodite Creatures: " + hermaphroditeCreatureCount.ToString();
        // Update the all time total hermaphrodite creature count text
        allTimeHermaphroditeCreatureCountText.text = "Total Hermaphrodite Creatures Born: " + allTimeHermaphroditeCreatureCount.ToString();
    }
}
