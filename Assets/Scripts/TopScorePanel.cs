using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopScorePanel : HUDStringDisplay
{

    private void OnEnable()
    {
        GameEvents.TopScoreChanged += UpdateTopScore;
    }

    private void OnDisable()
    {
        GameEvents.TopScoreChanged -= UpdateTopScore;
    }

    protected override void Awake()
    {
        base.Awake();
        
        UpdateTopScore();

    }

    private void UpdateTopScore()
    {
        DisplayString(GameManager.Instance.topScore.ToString());
    }

}
