using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController: MonoBehaviour
{

    public static LevelController Instance { get; private set; }

    public FloatingScoreManager floatingScoreManager;

    private bool finishedLevelLoading = true;
    private bool inLevel = false;
    private bool isPaused = false;

    public bool FinishedLevelLoading
    {
        get
        {
            return finishedLevelLoading;
        }
    }

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
    //private Coroutine timerRunning;
    public float lastingTimeToWarn;
    private bool isWarningTimer = false;

    // winning condition
    private List<Enemy> winningConditionEnemies = new List<Enemy>();
    private bool reachedEnd = false;
    public delegate IEnumerator Routine();
    private Routine EndLevelRoutine;
    private Routine StartLevelRoutine;

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
        if (inLevel)
        {
            CheckEnemyWinningCondition();
            CheckTimer();
            CheckReturnToMenu();
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void CheckReturnToMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndLevel();
            GameManager.Instance.ReturnToMenu(0f);
        }
    }


    private void CheckTimer()
    {
        if (timer <= lastingTimeToWarn && !isWarningTimer)
        {
            isWarningTimer = true;
            GameEvents.Level.TimerStarted.SafeCall();

        } else if (timer >= lastingTimeToWarn)
        {
            isWarningTimer = false;
        }
    }


    // starts a level
    public void PrepareToStartLevel()
    {
        finishedLevelLoading = false;

        StartCoroutine(RoutineOnLevelStart());

    }

    public void StartLevel(bool countTime)
    {
        inLevel = true;
        reachedEnd = false;
        isWarningTimer = false;
        timer = GameManager.Instance.defaultLevelTime;

        if (countTime)
        {
            //timerRunning = StartCoroutine(CountTimer());
            StartCoroutine(CountTimer());
        }

        // trigger event
        GameEvents.Level.LevelStarted.SafeCall();
        GameEvents.Level.PlayerStarted.SafeCall(GameManager.Instance.currentPlayer.player);

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

    public void ExtendTime(int newTimer)
    {
        timer = newTimer;
        GameEvents.Level.TimerExtended.SafeCall();
    }


    public void AddEnemyToWinningConditionList(Enemy enemy)
    {
        winningConditionEnemies.Add(enemy);
    }

    public void RemoveEnemyToWinningConditionList(Enemy enemy)
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

            StartCoroutine(RoutineOnLevelEnd());
        }
    }

    private IEnumerator RoutineOnLevelEnd()
    {
        yield return StartCoroutine(EndLevelRoutine());

        GameManager.Instance.AdvanceLevel();
    }

    private IEnumerator RoutineOnLevelStart()
    {
        if(StartLevelRoutine!=null)
            yield return StartCoroutine(StartLevelRoutine());

        finishedLevelLoading = true;

        StartLevel(true);
    }



    public void SetupLevelRoutine(StageStartOrEndAction.RoutinePosition routinePosition, Routine enumerator)
    {
        switch (routinePosition)
        {
            case StageStartOrEndAction.RoutinePosition.Start:
                StartLevelRoutine = enumerator;
                break;
            case StageStartOrEndAction.RoutinePosition.End:
                EndLevelRoutine = enumerator;
                break;
            default:
                break;
        }
        
    }

}
