using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScorePanel : HUDStringDisplay
{

    public PlayerID player;

    private void OnEnable()
    {
        GameEvents.ScoreIncremented += UpdateScore;
    }

    private void OnDisable()
    {
        GameEvents.ScoreIncremented -= UpdateScore;
    }

    protected override void Awake()
    {
        base.Awake();

        if (GameManager.Instance.currentGameMode == GameMode.SinglePlayer && player != PlayerID.Player1)
        {
            gameObject.SetActive(false);
            return;
        }

        UpdateScore();

    }
    public void UpdateScore()
    {
        DisplayString(GameManager.Instance.playersDictionary[player].score.ToString());
    }

    
}
