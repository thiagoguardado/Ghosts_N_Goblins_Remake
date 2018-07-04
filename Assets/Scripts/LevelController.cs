using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController: MonoBehaviour
{

    public static LevelController Instance { get; private set; }

    private bool inLevel = false;
    public bool InLevel
    {
        get
        {
            return inLevel;
        }
    }

    public FloatingScoreManager floatingScoreManager;

    
    private float timer = 0f;

    public int Lifes
    {
        get
        {
            return GameManager.Instance.currentPlayer.lifes;
        }
    }
    public int Score
    {
        get
        {
            return GameManager.Instance.currentPlayer.score;
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
        GameEvents.Player.PlayerDied += PlayerDies;
    }

    void OnDisable()
    {
        GameEvents.Player.PlayerDied -= PlayerDies;
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


    // starts a level
    public void StartLevel(bool countTime)
    {
        inLevel = true;
        timer = GameManager.Instance.defaultLevelTime;

        if(countTime)
            timerRunning = StartCoroutine(CountTimer());

        // trigger event
        GameEvents.Level.LevelStarted.SafeCall();

    }

    // ends level
    public void EndLevel()
    {
        inLevel = false;
    }

    public void PlayerDies()
    {
        EndLevel();

        // decrease life and restart level
        GameManager.Instance.SetupNextTry();
    }

    IEnumerator CountTimer() {

        while (inLevel)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = 0f;
                EndLevel();
                GameEvents.Level.TimeEnded.SafeCall();
                yield break;
            }

            yield return null;
        }

    }

    public void IncrementScore(int scoreIncrement)
    {
        GameManager.Instance.currentPlayer.IncrementScore(scoreIncrement);
        GameEvents.Score.ScoreIncremented.SafeCall();
    }

}
