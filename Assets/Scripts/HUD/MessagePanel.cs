using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour {

    public Transform lettersParent;
    public Image letterPrefab;
    public FontImages pinkFontDictionary;
    public FontImages yellowFontDictionary;

    private void Awake()
    {
        HidePanel();
    }

    private void OnEnable()
    {
        GameEvents.Level.PlayerReachedEnd += ShowFinalMessage;
        GameEvents.Level.PlayerStarted += ShowPlayerStartMessage;
        GameEvents.Level.PlayerGameOver += ShowPlayerGameOverMessage;
    }

    private void OnDisable()
    {
        GameEvents.Level.PlayerReachedEnd -= ShowFinalMessage;
        GameEvents.Level.PlayerStarted -= ShowPlayerStartMessage;
        GameEvents.Level.PlayerGameOver -= ShowPlayerGameOverMessage;
    }


    private void ShowFinalMessage()
    {
        ShowPanel("TAKE A KEY FOR COMING IN!", pinkFontDictionary);
    }

    private void ShowPlayerStartMessage(PlayerID playerID)
    {
        if (GameManager.Instance.currentGameMode == GameMode.SinglePlayer)
            return;

        int playerNumber = GetPlayerNumber(playerID);

        if (playerNumber < 0)
            return;

        ShowPanel("PLAYER " + playerNumber.ToString() + " START !", yellowFontDictionary);

        this.WaitAndAct(3.0f, () => HidePanel());
    }

    private void ShowPlayerGameOverMessage(PlayerID playerID)
    {
        int playerNumber = GetPlayerNumber(playerID);

        if (playerNumber < 0)
            return;

        ShowPanel("PLAYER " + playerNumber.ToString() + " GAME OVER !", yellowFontDictionary);

        this.WaitAndAct(3.0f, () => HidePanel());
    }

    private int GetPlayerNumber(PlayerID playerID)
    {
        switch (playerID)
        {
            case PlayerID.Player1:
                return 1;
            case PlayerID.Player2:
                return 2;
            default:
                return 0;
        }
    }

    private void ShowPanel(string sentence, FontImages fontDictionary)
    {
        ClearSentence();
        for (int i = 0; i < sentence.Length; i++)
        {
            Image img = Instantiate(letterPrefab, lettersParent);
            Sprite sprite = fontDictionary.GetSprite(sentence[i]);

            if (sprite == null)
            {
                Debug.LogError("Could not fill panel sentence, did not find " + sentence[i] + " on dictionary");
                ClearSentence();
                return;
            }
            img.sprite = sprite;
        }

        lettersParent.gameObject.SetActive(true);

    }

    private void HidePanel()
    {
        ClearSentence();
    }

    private void ClearSentence()
    {
        for (int i = 0; i < lettersParent.childCount; i++)
        {
            Destroy(lettersParent.GetChild(i).gameObject);
        }
    }
}
