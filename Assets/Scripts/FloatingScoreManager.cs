using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatingScoreManager",menuName ="",order =0)]
public class FloatingScoreManager : ScriptableObject {

    [System.Serializable]
    public struct ScoreAndSprite
    {
        public string score;
        public Sprite sprite;
    }

    public struct ScoreSprites
    {
        public Sprite left;
        public Sprite right;
    }

    public List<ScoreAndSprite> scoreAndSprite = new List<ScoreAndSprite>();

    public FloatingScore floatingScore;
    public float timeOnDisplay = 5f;
    
    public void InstantiateFloatingSscore(int score, Vector3 position)
    {
        Sprite left;
        Sprite right;

        if (LevelController.Instance.floatingScoreManager.GetSprites(score, out left, out right))
        {

            FloatingScore fs = Instantiate(floatingScore, position, Quaternion.identity);

            fs.spriteLeft.sprite = left;
            fs.spriteRight.sprite = right;

            fs.WaitAndAct(timeOnDisplay, () => Destroy(fs.gameObject));
        }
        else {
            Debug.LogError("FloatingScoreManager could not display score " + score.ToString() + " on screen");
        }

    }

    private bool GetSprites(int score, out Sprite left, out Sprite right) {

        left = null;
        right = null;

        if (score > 80000)
        {
            return false;
        }

        if (score <= 100)
        {
            for (int i = 0; i < scoreAndSprite.Count; i++)
            {
                if (scoreAndSprite[i].score == score.ToString())
                {
                    left = scoreAndSprite[i].sprite;
                    return true;
                }
            }
        }
        else
        {
            string leftScore = score.ToString().Substring(0, 2);
            string rightScore = score.ToString().Substring(2);

            bool foundLeft = false;
            bool foundRight = false;

            for (int i = 0; i < scoreAndSprite.Count; i++)
            {
                if (!foundLeft && scoreAndSprite[i].score == leftScore)
                {
                    left = scoreAndSprite[i].sprite;
                    foundLeft = true;
                }
                if (!foundRight && scoreAndSprite[i].score == rightScore)
                {
                    right = scoreAndSprite[i].sprite;
                    foundRight = true;
                }
            }

            if (foundLeft && foundRight)
            {
                return true;
            }
            else
            {
                left = null;
                right = null;
                return false;
            }

        }

        return false;

    }

}
