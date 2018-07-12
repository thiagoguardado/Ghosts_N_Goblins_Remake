using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {


        

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartNewGame(GameMode.SinglePlayer);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartNewGame(GameMode.SinglePlayer);
        }

    }

    void StartNewGame(GameMode gameMode)
    {

        GameEvents.GameManager.NewGameStarted.SafeCall();

        this.WaitAndAct(1f, () => GameManager.Instance.SetupNewGame(gameMode));
    }
}
