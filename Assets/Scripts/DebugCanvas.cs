using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebugCanvas : MonoBehaviour {

    public bool autoStart = false;
    public bool countTime = true;
    public Text centerText;
    public Button startButton;

    private void OnEnable()
    {
        GameEvents.TimeEnded += DisplayLoseText;
    }

    private void OnDisable()
    {
        GameEvents.TimeEnded -= DisplayLoseText;
    }

    private void Start()
    {
        centerText.text = "";

        if (autoStart)
        {
            PressStartButton();
        }
    }


    public void PressStartButton()
    {
        GameController.Instance.NewGame(countTime);
        startButton.gameObject.SetActive(false);
        centerText.text = "";

    }

    private void DisplayLoseText()
    {
        DisplayCenterText("LOSE");
        startButton.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void DisplayCenterText(string text)
    {
        centerText.text = text;
    }


}
