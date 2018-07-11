using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController: MonoBehaviour
{

    public static LevelController Instance { get; private set; }

    public FloatingScoreManager floatingScoreManager;
    public Key keyObject;

    private bool inLevel = false;
    public bool InLevel
    {
        get
        {
            return inLevel;
        }
    }

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

    // timer
    private float timer = 0f;
    private Coroutine timerRunning;

    // winning condition
    private List<Enemy> winningConditionEnemies = new List<Enemy>();
    private bool reachedEnd = false;
    public delegate IEnumerator Routine();
    private Routine EndLevelRoutine;

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

    private void Update()
    {
        CheckEnemyWinningCondition();
    }


    // starts a level
    public void StartLevel(bool countTime)
    {
        inLevel = true;
        reachedEnd = false;
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

    public void GetScoreBonus(int scoreBonus, Vector3 bonusPosition)
    {
        IncrementScore(scoreBonus);
        GameEvents.Score.BonusScore(bonusPosition, scoreBonus);

    }

    public void ExtendTime(int timeToAdd)
    {
        timer += timeToAdd;
        GameEvents.Level.TimerExtended.SafeCall();
    }


    public void AddEnemyToWinningCOnditionList(Enemy enemy)
    {
        winningConditionEnemies.Add(enemy);
    }

    public void RemoveEnemyToWinningCOnditionList(Enemy enemy)
    {
        winningConditionEnemies.Remove(enemy);
    }


    private void CheckEnemyWinningCondition()
    {
        if (winningConditionEnemies.Count <= 0 && !reachedEnd)
        {
            //win
            reachedEnd = true;
            EndLevel();
            GameEvents.Level.PlayerReachedEnd.SafeCall();

            StartCoroutine(StartEndLevel());
        }
    }

    private IEnumerator StartEndLevel()
    {

        yield return StartCoroutine(EndLevelRoutine());

        GameManager.Instance.AdvanceLevel();

    }

    public void SetupLevelRoutine(Routine enumerator)
    {
        EndLevelRoutine = enumerator;
    }

}
