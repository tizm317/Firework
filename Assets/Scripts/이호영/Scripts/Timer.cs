using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Timer
    // 이호영
    //UI
    public Text text_Timer;

    //Time
    float time_current;
    float time_Max = 300f; // 5분
    float minute;
    float second;

    bool isEnded;
    private void Start()
    {
        Reset_Timer();
    }
    void Update()
    {
        if (isEnded)
            return;

        Check_Timer();
    }
    private void Check_Timer()
    {
        if (0 < time_current)
        {
            time_current -= Time.deltaTime;
            minute = (int)(time_current / 60);
            second = (int)(time_current % 60);
            text_Timer.text = minute + ":" + second.ToString("00"); // 0:00 format
        }
        else if (!isEnded)
        {
            End_Timer();
        }
    }
    private void End_Timer()
    {
        time_current = 0;
        minute = (int)(time_current / 60);
        second = (int)(time_current % 60);
        text_Timer.text = minute + ":" + second.ToString("00");
        GameManager.singleTon.GameOver();
        isEnded = true;
    }

    private void Reset_Timer()
    {
        time_current = time_Max;
        minute = (int)(time_current / 60);
        second = (int)(time_current % 60);
        text_Timer.text = minute + ":" + second.ToString("00");
        isEnded = false;
    }
}
