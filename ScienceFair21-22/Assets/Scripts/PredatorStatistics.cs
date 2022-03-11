using UnityEngine;
using UnityEngine.UI;

public class PredatorStatistics : MonoBehaviour
{
    // STATIC VARIABLES FOR TRACKING
    // Sexual
    public static int predatorSexualCount;
    public static int allTimePredatorSexualCount;
    // Asexual
    public static int predatorAsexualCount;
    public static int allTimePredatorAsexualCount;
    // Hermaphrodite
    public static int predatorHermaphroditeCount;
    public static int allTimePredatorHermaphroditeCount;
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
    [Header("Pred. Sexual Stat Text")]
    public Text predatorSexualCountText;
    public Text allTimePredatorSexualCountText;
    // Asexual
    [Header("Pred. Asexual Stat Text")]
    public Text predatorAsexualCountText;
    public Text allTimePredatorAsexualCountText;
    // Hermaphrodite
    [Header("Pred. Hermaphrodite Stat Text")]
    public Text predatorHermaphroditeCountText;
    public Text allTimePredatorHermaphroditeCountText;

    // COLLECTING DATA
    [Header("Collecting Data")]
    public float setRecordInterval;
    // run time effected countdown
    private float recordInterval;
    // reference to the SaveData script allowing for writing to logs
    public SaveData saveData;
    // reference to the Timer script allowing for access to it's values
    public Timer timer;

    private void Start()
    {
        // RESET ALL VALUES
        // this ensures that when the functions used to "reload" the scene static statistic values start at 0
        // Reset all time stats
        allTimePredatorSexualCount = 0;
        allTimePredatorAsexualCount = 0;
        allTimePredatorHermaphroditeCount = 0;
        // Reset current stats
        predatorSexualCount = 0;
        predatorAsexualCount = 0;
        predatorHermaphroditeCount = 0;

        // Initialize trait division tracker arrays
        // Size
        sizeDivisionTracker = new int[5];
        // Speed
        speedDivisionTracker = new int[5];
        // Sense Radius
        senseRadiusDivisionTracker = new int[5];
    }

    // Set RecordingInterval
    public void SetRecordingInterval()
    {
        // Set recordingInterval 
        setRecordInterval = MenuManager.setDataRecordingInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // Update relevant statistics of PREY
        if (MenuManager.activePredator[0])
            SexualCreatureTextUpdate();
        if (MenuManager.activePredator[1])
            AsexualCreatureTextUpdate();
        if (MenuManager.activePredator[2])
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
            if (MenuManager.activePredator[0])
                LogSexual();
            if (MenuManager.activePredator[1])
                LogAsexual();
            if (MenuManager.activePredator[2])
                LogHermaphrodite();
            // Log trait stats
            LogSize();
            LogSpeed();
            LogSenseRadius();
        }
    }

    // LOGGING FUNCTIONS
    // the following functions tell saveData what new content to add to the file "Log.txt"
    // Asexual
    void LogAsexual()
    {
        saveData.CreatePredatorPopulationLog("Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Predators: " + predatorAsexualCount.ToString() + " All Time Predators:  " + allTimePredatorAsexualCount.ToString());
    }
    // Sexual
    void LogSexual()
    {
        saveData.CreatePredatorPopulationLog("Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Predators: " + predatorSexualCount.ToString() + " All Time Predators: " + allTimePredatorSexualCount.ToString());
    }
    // Hermaphrodite
    void LogHermaphrodite()
    {
        saveData.CreatePredatorPopulationLog("Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Predators: " + predatorHermaphroditeCount.ToString() + " All Time Predators: " + allTimePredatorHermaphroditeCount.ToString());
    }
    // TRAIT STATS
    // Size
    void LogSize()
    {
        saveData.CreatePredatorSizeLog("Size: Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Division 1: " + sizeDivisionTracker[0].ToString() + " Division 2: " + sizeDivisionTracker[1].ToString() + " Division 3: " + sizeDivisionTracker[2].ToString() + " Division 4: " + sizeDivisionTracker[3].ToString() + " Division 5: " + sizeDivisionTracker[4].ToString());
    }
    // Speed
    void LogSpeed()
    {
        saveData.CreatePredatorSpeedLog("Speed: Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Division 1: " + speedDivisionTracker[0].ToString() + " Division 2: " + speedDivisionTracker[1].ToString() + " Division 3: " + speedDivisionTracker[2].ToString() + " Division 4: " + speedDivisionTracker[3].ToString() + " Division 5: " + speedDivisionTracker[4].ToString());
    }
    // Sense Radius
    void LogSenseRadius()
    {
        saveData.CreatePredatorSenseRadiusLog(" Sense Radius: Time: " + Mathf.FloorToInt(Timer.time).ToString() + " Division 1: " + senseRadiusDivisionTracker[0].ToString() + " Division 2: " + senseRadiusDivisionTracker[1].ToString() + " Division 3: " + senseRadiusDivisionTracker[2].ToString() + " Division 4: " + senseRadiusDivisionTracker[3].ToString() + " Division 5: " + senseRadiusDivisionTracker[4].ToString());
    }

    // Update sexual statistics text
    void SexualCreatureTextUpdate()
    {
        // SEXUAL STATS
        // Update the total sexual creature count text
        predatorSexualCountText.text = "Total Pred. Sexual: " + predatorSexualCount.ToString();
        // Update the all time total sexual creature count text
        allTimePredatorSexualCountText.text = "Total Pred. Sexual Born: " + allTimePredatorSexualCount.ToString();
    }

    // Update asexual statistics text
    void AsexualCreatureTextUpdate()
    {
        // ASEXUAL STATS
        // Update the total asexual creature count text
        predatorAsexualCountText.text = "Total Pred. Asexual: " + predatorAsexualCount.ToString();
        // Update the all time total asexual creature count text
        allTimePredatorAsexualCountText.text = "Total Pred. Asexual Born: " + allTimePredatorAsexualCount.ToString();
    }

    // Update hermaphrodite statistics text
    void HermaphroditeCreatureTextUpdate()
    {
        // HERMAPHRODITE STATS
        // Update the total hermaphrodite creature count text
        predatorHermaphroditeCountText.text = "Total Pred. Hermaphrodite: " + predatorHermaphroditeCount.ToString();
        // Update the all time total hermaphrodite creature count text
        allTimePredatorHermaphroditeCountText.text = "Total Pred. Hermaphrodite Born: " + allTimePredatorHermaphroditeCount.ToString();
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
