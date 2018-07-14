using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1EndLevelAction : StageStartOrEndAction
{

    public Key keyPrefab;
    public Door doors;
    public Transform playerPositionWhenDoorOpens;
    public Transform playerPositionEnteringDoor;
    

    protected override IEnumerator Routine()
    {
        // key
        yield return StartCoroutine(MoveKeyDown());

        // move player
        yield return StartCoroutine(MovePlayerTo(playerPositionWhenDoorOpens));

        // open gates
        PlayerController.Instance.GetArmor();
        PlayerController.Instance.StartVictoryPose();
        yield return StartCoroutine(OpenGatesDoor());

        // move player
        PlayerController.Instance.StopVictoryPose();
        yield return StartCoroutine(MovePlayerTo(playerPositionEnteringDoor));

    }

    private IEnumerator OpenGatesDoor()
    {
        doors.StartOpening();

        while (!doors.Finished)
        {
            yield return null;
        }

    }

    private IEnumerator MovePlayerTo(Transform playerPositionWhenDoorOpens)
    {

        while (Mathf.Abs(PlayerController.Instance.transform.position.x - playerPositionWhenDoorOpens.position.x) > 0.01f)
        {
            PlayerController.Instance.SimulateHorizontalAxis((PlayerController.Instance.transform.position.x - playerPositionWhenDoorOpens.position.x) < 0 ? 1f : -1f);

            yield return null;
        }

        PlayerController.Instance.SimulateHorizontalAxis(0f);

    }

    private IEnumerator MoveKeyDown()
    {
        Key key = Instantiate(keyPrefab, PlayerController.Instance.transform.position, keyPrefab.transform.rotation);

        while (!key.Finished)
        {
            yield return null;
        }

        Destroy(key.gameObject);

    }
	
}
