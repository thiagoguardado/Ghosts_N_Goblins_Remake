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

    public Player(PlayerID player, int score, int lifes)
    {
        this.player = player;
        this.score = score;
        this.lifes = lifes;
    }

    public void IncrementScore(int increment)
    {
        score += increment;
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
    public int topScore = 10000;

    [Header("Singleplayer")]
    public int initialLivesOnSinglePlayer = 3;

    [Header("Multiplayer")]
    public int initialLivesOnMultiPlayer = 2;

    private const float waitTimeBeforeRetry = 5f;

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
        if (currentPlayer.score >= topScore)
        {
            topScore = currentPlayer.score;
            GameEvents.Score.TopScoreChanged.SafeCall();
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

    private void CreatePlayers()
    {
        players = new List<Player>();

        switch (currentGameMode)
        {
            case GameMode.SinglePlayer:
                players.Add(new Player(PlayerID.Player1, 0, initialLivesOnSinglePlayer));
                break;
            case GameMode.MultiPlayer:
                players.Add(new Player(PlayerID.Player1, 0, initialLivesOnMultiPlayer));
                players.Add(new Player(PlayerID.Player2, 0, initialLivesOnMultiPlayer));
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


    public void SetupNextTry()
    {
        // find next player
        Player nextPlayer = players[(players.IndexOf(currentPlayer) + 1) % players.Count];

        // deacrease life count
        currentPlayer.lifes -= 1;
        if (currentPlayer.lifes < 0)
        {
            GameEvents.Player.PlayerGameOver.SafeCall();
            players.Remove(currentPlayer);
        }
        else {
            GameEvents.Player.PlayerLoseLife.SafeCall();
        }

        if (players.Count <= 0)
        {
            // end game
            return;
        }

        // change player
        currentPlayer = nextPlayer;

        // reload scene after waiting for some time
        string scene = SceneManager.GetActiveScene().name;
        MyExtensions.WaitAndAct(this, waitTimeBeforeRetry, () => SceneManager.LoadScene(scene));
        

    }

}
