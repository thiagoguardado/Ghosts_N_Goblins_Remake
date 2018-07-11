using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerPanel : HUDStringDisplay
{

    private void Update()
    {

        // update timer
        UpdateTimer();

    }

    // update timer display
    private void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(LevelController.Instance.Timer / 60);
        int seconds = Mathf.CeilToInt(LevelController.Instance.Timer % 60);
        if (seconds == 60)
        {
            minutes += 1;
            seconds = 0;
        }

        DisplayString(minutes.ToString("0") + ":" + seconds.ToString("00"));

    }
}
