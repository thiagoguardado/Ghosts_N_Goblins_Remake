using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScorePanel : HUDStringDisplay
{

    private void OnEnable()
    {
        GameEvents.ScoreIncremented += UpdateScore;
    }

    private void OnDisable()
    {
        GameEvents.ScoreIncremented -= UpdateScore;
    }


    public void UpdateScore()
    {
        DisplayString(GameController.Instance.Score.ToString());
    }

    
}
