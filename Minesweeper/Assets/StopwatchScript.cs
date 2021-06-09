using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchScript : MonoBehaviour
{
    public bool stopWatchActive = false;
    public float currentTime;
    public Text currentTimeText;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if(stopWatchActive) {
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
