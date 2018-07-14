using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerID
{
    Player1,
    Player2
}

public class Player
{
    public PlayerID player;
    public int score = 0;
    public int lifes = 0;
    public bool reachedHighScore = false;
    public bool hasStarted = false;
    private Queue<int> nextExtraLives;


    public Player(PlayerID player, int score, int lifes, List<int> nextExtraLives)
    {
        this.player = player;
        this.score = score;
        this.lifes = lifes;
        this.nextExtraLives = new Queue<int>(nextExtraLives);
        reachedHighScore = false;
    }

    public void IncrementScore(int increment)
    {
        score += increment;

        if (score >= nextExtraLives.Peek())
        {
            nextExtraLives.Dequeue();
            IncrementLife(1);
        }
    }

    public void IncrementLife(int increment)
    {
        lifes += 1;
        GameEvents.Player.PlayerGetsExtraLife.SafeCall();
    }

}

public enum GameMode
{
    SinglePlayer,
    MultiPlayer
}

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }


    public GameMode currentGameMode = GameMode.SinglePlayer;
    public List<Player> players { get; private set; }
    public Player currentPlayer { get; private set; }
    public Dictionary<PlayerID, Player> playersDictionary = new Dictionary<PlayerID, Player>();
    public float defaultLevelTime = 120f;
    public int defaultTopScore = 10000;
    public List<int> defaultNextExtraLives;

    [Header("Singleplayer")]
    public int initialLivesOnSinglePlayer = 3;

    [Header("Multiplayer")]
    public int initialLivesOnMultiPlayer = 2;

    private const float waitTimeBeforeRetry = 5f;
    private const float waitTimeAfterDeath = 9f;

    private void OnEnable()
    {
        GameEvents.Score.ScoreIncremented += CheckTopScore;
    }

    private void OnDisable()
    {
        GameEvents.Score.ScoreIncremented -= CheckTopScore;
    }

    private void CheckTopScore()
    {
        if (currentPlayer.score >= defaultTopScore)
        {
            defaultTopScore = currentPlayer.score;
            GameEvents.Score.TopScoreChanged.SafeCall();

            if (!currentPlayer.reachedHighScore)
            {
                currentPlayer.reachedHighScore = true;
                GameEvents.Score.PlayerReachedHighScore.SafeCall();
            }
        }
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

        CreatePlayers();
    }

    public void SetupNewGame(GameMode gameMode)
    {
        currentGameMode = gameMode;

        CreatePlayers();

        StartCoroutine(StartLevel("Stage1"));
    }

    private void CreatePlayers()
    {
        players = new List<Player>();
        playersDictionary.Clear();

        switch (currentGameMode)
        {
            case GameMode.SinglePlayer:
                players.Add(new Player(PlayerID.Player1, 0, initialLivesOnSinglePlayer, defaultNextExtraLives));
                break;
            case GameMode.MultiPlayer:
                players.Add(new Player(PlayerID.Player1, 0, initialLivesOnMultiPlayer, defaultNextExtraLives));
                players.Add(new Player(PlayerID.Player2, 0, initialLivesOnMultiPlayer, defaultNextExtraLives));
                break;
            default:
                break;
        }


        for (int i = 0; i < players.Count; i++)
        {
            playersDictionary.Add(players[i].player, players[i]);
        }

        currentPlayer = players[0];
    }

    public void AdvanceLevel()
    {

        // find next player
        Player nextPlayer = players[(players.IndexOf(currentPlayer) + 1) % players.Count];

        players.Remove(currentPlayer);

        if (players.Count <= 0)
        {
            // end game
            ReturnToMenu();
            return;
        }

        // change player
        SetupNextPlayer(nextPlayer);

        // reload scene after waiting for some time
        string scene = SceneManager.GetActiveScene().name;
        MyExtensions.WaitAndAct(this, waitTimeBeforeRetry, () => StartCoroutine(StartLevel(scene)));

    }



    public void SetupNextTry()
    {
        // find next player
        Player nextPlayer = players[(players.IndexOf(currentPlayer) + 1) % players.Count];

        float waitingTime;

        // deacrease life count
        currentPlayer.lifes -= 1;
        if (currentPlayer.lifes < 0)
        {
            GameEvents.Level.PlayerGameOver.SafeCall(currentPlayer.player);
            waitingTime = waitTimeAfterDeath;
            players.Remove(currentPlayer);
        }
        else {
            waitingTime = waitTimeBeforeRetry;
            GameEvents.Player.PlayerLoseLife.SafeCall();
        }

        if (players.Count <= 0)
        {
            // end game
            ReturnToMenu();
            return;
        }

        // change player
        SetupNextPlayer(nextPlayer);

        // reload scene after waiting for some time
        string scene = SceneManager.GetActiveScene().name;
        MyExtensions.WaitAndAct(this, waitingTime, () => StartCoroutine(StartLevel(scene)));

    }


    private void SetupNextPlayer(Player nextPlayer)
    {
        currentPlayer = nextPlayer;

        currentPlayer.reachedHighScore = currentPlayer.score >= defaultTopScore;

    }

    private void ReturnToMenu()
    {
        MyExtensions.WaitAndAct(this, waitTimeAfterDeath, () => SceneManager.LoadScene("Menu"));
    }

    private IEnumerator StartLevel(string levelName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(levelName);

        while (!async.isDone)
        {
            yield return null;
        }

        LevelController.Instance.PrepareToStartLevel();
    }
}
