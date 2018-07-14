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
    }

    private void OnDisable()
    {
        GameEvents.Level.PlayerReachedEnd -= ShowFinalMessage;
        GameEvents.Level.PlayerStarted -= ShowPlayerStartMessage;
    }


    private void ShowFinalMessage()
    {
        ShowPanel("TAKE A KEY FOR COMING IN!", pinkFontDictionary);
    }

    private void ShowPlayerStartMessage(PlayerID playerID)
    {
        int playerNumber;
        switch (playerID)
        {
            case PlayerID.Player1:
                playerNumber = 1;
                break;
            case PlayerID.Player2:
                playerNumber = 2;
                break;
            default:
                return;
        }
        ShowPanel("PLAYER " + playerNumber.ToString() + " START !", yellowFontDictionary);

        this.WaitAndAct(3.0f, () => HidePanel());
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
