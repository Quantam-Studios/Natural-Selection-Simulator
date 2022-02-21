using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // TEXT OBJECTS FOR DISPLAYING STATISTICS
    public Text sexualCreatureCountText;
    public Text maleSexualCreatureCountText;
    public Text femaleSexualCreatureCountText;
    public Text allTimeSexualCreatureCountText;
    public Text allTimeMaleSexualCreatureCountText;
    public Text allTimeFemaleSexualCreatureCountText;

    private void Start()
    {
        // Make the game run as fast as possible
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
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
}
