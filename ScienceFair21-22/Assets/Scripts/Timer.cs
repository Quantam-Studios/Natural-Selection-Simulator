using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // TEXT OBJECTS FOR DISPLAYING TIME
    [Header("General")]
    public Text timerText;
    public static float time = 0;

    // VALUES OF TIME FOR THE TIME LIMIT
    [Header("Time Limit")]
    public float[] timeLimits;
    public int activeTimeLimitIndex;
    // reference to menu manager for resetting
    public MenuManager menuManager;

    // Update is called once per frame
    void Update()
    {
        // Update time value
        time += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(0 + time);
        // Format the time 
        string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        timerText.text = timeText;

        // Time Limit
        // if time limit is reached and there is a finite time limit then end if the simulation.
        if (time >= timeLimits[activeTimeLimitIndex] && activeTimeLimitIndex != 8)
            // end simulation
            menuManager.EndSimulation();
    }
}
