using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1StartLevelAction : StageStartOrEndAction
{

    public StartAnimation startAnimation;

    protected override IEnumerator Routine()
    {
        if (GameManager.Instance.currentPlayer.hasStarted)
        {

            Destroy(startAnimation.gameObject);
            yield break;
        }

        // mark player start
        GameManager.Instance.currentPlayer.hasStarted = true;

        // notify
        GameEvents.Level.OpeningStarted.SafeCall();

        // start animation
        startAnimation.gameObject.SetActive(true);

        while (!startAnimation.finished)
        {
            yield return null;
        }

        Destroy(startAnimation.gameObject);
        
    }
    
}
