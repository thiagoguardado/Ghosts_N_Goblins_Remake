using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScore : MonoBehaviour {

    public int score;

    public void IncrementGameScore()
    {
        GameController.Instance.IncrementScore(score);

    }

    public void IncrementGameScore(Vector3 displayScoreTextPosition)
    {
        GameController.Instance.IncrementScore(score);

        DisplayScore(displayScoreTextPosition);
    }

    private void DisplayScore(Vector3 displayScoreTextPosition)
    {
        GameController.Instance.floatingScoreManager.InstantiateFloatingSscore(score, displayScoreTextPosition);
    }
}
