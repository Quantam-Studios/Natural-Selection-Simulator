using UnityEngine;
using UnityEngine.UI;
using System;

public class CreatureStatistics : MonoBehaviour
{
    // STATIC VARIABLES FOR TRACKING
    // POPULATIONS
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
    // TRAITS
    [Header("Trait Tracking")]
    // Size
    public float[] sizeDivisions;
    public static int[] sizeDivisionTracker;
    // Speed
    public float[] speedDivisions;
    public static int[] speedDivisionTracker;
    // Sense Radius
    public float[] senseRadiusDivisions;
    public static int[] senseRadiusDivisionTracker;

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

        // Initialize trait division tracker arrays
        // Size
        sizeDivisionTracker = new int[5];
        // Speed
        speedDivisionTracker = new int[5];
        // Sense Radius
        senseRadiusDivisionTracker = new int[5];
    }

    // INITIALIZE / FORMAT NEW SET OF DATA
    // only called in the RunSimulation() function of MenuManager
    // this ensures that all values, and settings chosen by the user have been set. 
    public void initializeNewLog()
    {
        // VERY IMPORTANT
        // create the new folder, and sub folder for this simulation's logs
        saveData.CreateNewLogFolder(MenuManager.logFolderName);
        // Set recordingInterval 
        setRecordInterval = MenuManager.setDataRecordingInterval;
        // Create a description of the circumstances on the log
        saveData.CreatePreyPopulationLog("\n\n New Data Set \n" + activeCreatureName[MenuManager.activeCreatureIndex] + " creatures with " + activePredatorName[MenuManager.activePredatorIndex] + " predators\n");
        // Determine and format time limit into a string
        TimeSpan timeSpan = TimeSpan.FromSeconds(0 + timer.timeLimits[timer.activeTimeLimitIndex]);
        string timeLimit = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        // Add all presets, and inital values to the log
        saveData.CreatePreyPopulationLog("Time Limit: " + timeLimit + " Inital Creature Count: " + MenuManager.initialCreatureCount.ToString() + " Initial Food Count: " + MenuManager.initalFood + " Food Spawn Rate: " + MenuManager.foodSpawnRate + "\n");
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
            // Log relevant statistics
            if (MenuManager.activeCreatures[0])
                LogSexual();
            if (MenuManager.activeCreatures[1])
                LogAsexual();
            if (MenuManager.activeCreatures[2])
                LogHermaphrodite();
            // Log trait stats
            LogSize();
            LogSpeed();
            LogSenseRadius();
        }
    }

    // LOGGING FUNCTIONS
    // POPULATION STATS
    // Asexual
    void LogAsexual()
    {
        saveData.CreatePreyPopulationLog("Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Creatures: " + asexualCreatureCount.ToString() + " All Time Creatures: " + allTimeAsexualCreatureCount.ToString());
    }
    // Sexual
    void LogSexual()
    {
        saveData.CreatePreyPopulationLog("Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Creatures: " + sexualCreatureCount.ToString() + " All Time Creatures: " + allTimeSexualCreatureCount.ToString());
    }
    // Hermaphrodite
    void LogHermaphrodite()
    {
        saveData.CreatePreyPopulationLog("Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Creatures: " + hermaphroditeCreatureCount.ToString() + " All Time Creatures: " + allTimeHermaphroditeCreatureCount.ToString());
    }
    // TRAIT STATS
    // Size
    void LogSize()
    {
        saveData.CreatePreySizeLog("Size: Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Division 1: " + sizeDivisionTracker[0].ToString() + " Division 2: " + sizeDivisionTracker[1].ToString() + " Division 3: " + sizeDivisionTracker[2].ToString() + " Division 4: " + sizeDivisionTracker[3].ToString() + " Division 5: " + sizeDivisionTracker[4].ToString());
    }
    // Speed
    void LogSpeed()
    {
        saveData.CreatePreySpeedLog("Speed: Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Division 1: " + speedDivisionTracker[0].ToString() + " Division 2: " + speedDivisionTracker[1].ToString() + " Division 3: " + speedDivisionTracker[2].ToString() + " Division 4: " + speedDivisionTracker[3].ToString() + " Division 5: " + speedDivisionTracker[4].ToString());
    }
    // Sense Radius
    void LogSenseRadius()
    {
        saveData.CreatePreySenseRadiusLog(" Sense Radius: Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Division 1: " + senseRadiusDivisionTracker[0].ToString() + " Division 2: " + senseRadiusDivisionTracker[1].ToString() + " Division 3: " + senseRadiusDivisionTracker[2].ToString() + " Division 4: " + senseRadiusDivisionTracker[3].ToString() + " Division 5: " + senseRadiusDivisionTracker[4].ToString());
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

    // DETERMINE DIVISION OF TRAITS
    // Size
    public int getSizeDivision(float size)
    {
        int division = 0;
        // compare size to divisions
        for (int i = 0; i < sizeDivisions.Length; i++)
        {
            // check if the size is smaller than or equal to the division
            if (size >= sizeDivisions[i])
                division = i;
            // if not then continue
        }
        // return the division for later use in 
        return division;
    }
    // Speed
    public int getSpeedDivision(float speed)
    {
        int division = 0;
        // compare speed to divisions
        for (int i = 0; i < speedDivisions.Length; i++)
        {
            // check if the speed is smaller than or equal to the division
            if (speed >= speedDivisions[i])
                division = i;
            // if not then continue
        }
        // return the division for later use in 
        return division;
    }
    // Sense Radius
    public int getSenseRadiusDivision(float senseRadius)
    {
        int division = 0;
        // compare sense radius to divisions
        for (int i = 0; i < senseRadiusDivisions.Length; i++)
        {
            // check if the sense radius is smaller than or equal to the division
            if (senseRadius >= senseRadiusDivisions[i])
                division = i;
            // if not then continue
        }
        // return the division for later use in 
        return division;
    }
}
