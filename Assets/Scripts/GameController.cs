using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController: MonoBehaviour
{

    public static GameController Instance { get; private set; }

    private bool inLevel = false;
    public bool InLevel
    {
        get
        {
            return inLevel;
        }
    }

    public int initialTimer = 120;
    public int initialLifes = 3;

    public FloatingScoreManager floatingScoreManager;

    private int lifes = 0;
    private int score = 0;
    private float timer = 0f;

    public int Lifes
    {
        get
        {
            return lifes;
        }
    }
    public int Score
    {
        get
        {
            return score;
        }
    }
    public float Timer
    {
        get
        {
            return timer;
        }
    }



    private Coroutine timerRunning;


    void OnEnable()
    {
        GameEvents.PlayerDied += EndLevel;
    }

    void OnDisable()
    {
        GameEvents.PlayerDied -= EndLevel;
    }

    private void Awake()
    {
        // singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(gameObject);
        }


    }

    // starts a game
    public void NewGame(bool countTime)
    {
        lifes = initialLifes;

        StartLevel(countTime);

    }

    // starts a level
    public void StartLevel(bool countTime)
    {
        inLevel = true;
        timer = (float)initialTimer;

        if(countTime)
            timerRunning = StartCoroutine(CountTimer());

        // trigger event
        GameEvents.LevelStarted.SafeCall();

    }

    // ends level
    public void EndLevel()
    {
        inLevel = false;
    }

    IEnumerator CountTimer() {

        while (inLevel)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = 0f;
                EndLevel();
                GameEvents.TimeEnded.SafeCall();
                yield break;
            }

            yield return null;
        }

    }

    public void IncrementScore(int scoreIncrement)
    {
        score += scoreIncrement;
        GameEvents.ScoreIncremented.SafeCall();
    }

}
