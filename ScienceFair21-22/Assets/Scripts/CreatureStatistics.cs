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
    // Aseuxal Creature Counts
    public static int asexualCreatureCount;
    public static int allTimeAsexualCreatureCount;
    // Hermaphrodite Creature Counts
    public static int hermaphroditeCreatureCount;
    public static int allTimeHermaphroditeCreatureCount;

    // TEXT OBJECTS FOR DISPLAYING STATISTICS
    // Sexual
    [Header("Sexual Stat Text")]
    public Text sexualCreatureCountText;
    public Text maleSexualCreatureCountText;
    public Text femaleSexualCreatureCountText;
    public Text allTimeSexualCreatureCountText;
    public Text allTimeMaleSexualCreatureCountText;
    public Text allTimeFemaleSexualCreatureCountText;
    // Asexual
    [Header("Asexual Stat Text")]
    public Text asexualCreatureCountText;
    public Text allTimeAsexualCreatureCountText;
    // Hermaphrodite
    [Header("Hermaphrodite Stat Text")]
    public Text hermaphroditeCreatureCountText;
    public Text allTimeHermaphroditeCreatureCountText;

    private void Start()
    {
        // Make the game run as fast as possible
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        // Update relevant statistics
        if (MenuManager.activeCreature[0])
            SexualCreatureTextUpdate();
        if (MenuManager.activeCreature[1])
            AsexualCreatureTextUpdate();
        if (MenuManager.activeCreature[2])
            HermaphroditeCreatureTextUpdate();
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
