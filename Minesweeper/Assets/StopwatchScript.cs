using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchScript : MonoBehaviour
{
    // Simple StopWatch script
    public bool stopWatchActive = false;
    public float currentTime;
    public Text currentTimeText;
    void Start()
    {
        currentTime = 0;

    }

    void Update()
    {
        if(stopWatchActive) {
            // Delta time is a paramter by Unity that hold the time between current frame and previous frame
            // Makes stopwatch Independent of frame rate
            currentTime += Time.deltaTime; 

            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            currentTimeText.text = time.ToString(@"mm\:ss\:f");
        }
    }

    public void startStopWatch() {
        stopWatchActive = true;
    }
    public void stopStopWatch() {
        stopWatchActive = false;
    }
}
