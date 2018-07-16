using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hacks : MonoBehaviour {

    public Text textBox;
    public float timeDisplayed;
    public Animator canvasGroupAnimator;
    public GameObject solidWaterObject;
    public GameObject pauseFadeObject;

    private Coroutine fadingCoroutine;
    private bool isInvincible = false;
    private bool isWaterSolid = false;
    private bool isPaused = false;

    private void OnEnable()
    {
        GameEvents.Level.LevelStarted += SetPlayerInvincibility;
    }

    private void OnDisable()
    {
        GameEvents.Level.LevelStarted -= SetPlayerInvincibility;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (LevelController.Instance.InLevel)
            {
                TriggerInvincibleMode();
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (LevelController.Instance.InLevel)
            {
                TriggerSolidWater();
            }
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            if (LevelController.Instance.InLevel)
            {
                Pause();
            }
        }

    }

    private void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1f;
        }

        pauseFadeObject.SetActive(isPaused);

        TriggerTextOnScreen("Paused: " + (isPaused ? "ON" : "OFF"));
    }

    private void TriggerSolidWater()
    {
        isWaterSolid = !isWaterSolid;

        SetSolidWater();

        TriggerTextOnScreen("Solid Water: " + (isWaterSolid ? "ON" : "OFF"));
    }


    private void TriggerInvincibleMode()
    {
        isInvincible = !isInvincible;

        SetPlayerInvincibility();

        TriggerTextOnScreen("Invincible Mode: " + (isInvincible ? "ON" : "OFF"));
        
    }

    private void TriggerTextOnScreen(string textToDisplay)
    {
        textBox.text = textToDisplay;

        canvasGroupAnimator.SetBool("Show", true);

        if (fadingCoroutine != null)
            StopCoroutine(fadingCoroutine);

        fadingCoroutine = StartCoroutine(WaitAndAct(timeDisplayed, () => canvasGroupAnimator.SetBool("Show", false)));
        
    }

    IEnumerator WaitAndAct(float duration, Action action)
    {
        yield return new WaitForSecondsRealtime(duration);

        action.Invoke();
    }

    private void SetPlayerInvincibility()
    {

        PlayerController.Instance.invincibleHack = isInvincible;

    }

    private void SetSolidWater()
    {
        solidWaterObject.SetActive(isWaterSolid);
    }

}
