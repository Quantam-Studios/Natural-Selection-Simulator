using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // TEXT OBJECTS FOR DISPLAYING TIME
    public Text timerText;
    public float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(0 + time);
        // Format the time 
        string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        timerText.text = timeText;
    }
}
