using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hacks : MonoBehaviour {

    public Text textBox;
    public float timeDisplayed;
    public Animator canvasGroupAnimator;

    private Coroutine fadingCoroutine;
    private bool isInvincible = false;


	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (LevelController.Instance.InLevel)
            {
                TriggerInvincibleMode();
            }
        }

	}

    private void TriggerInvincibleMode()
    {
        isInvincible = !isInvincible;

        PlayerController.Instance.invincibleHack = isInvincible;

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
        yield return new WaitForSeconds(duration);

        action.Invoke();
    }
}
