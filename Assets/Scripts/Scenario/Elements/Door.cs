using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public float timeToOpen;
    public Transform leftDoorDestination;
    public Transform rightDoorDestination;
    public Transform leftDoor;
    public Transform rightDoor;
    public Transform bonusPosition;
    public int bonusValue;

    private bool finished;

    public bool Finished
    {
        get
        {
            return finished;
        }
    }

    public void StartOpening()
    {
        finished = false;

        GameEvents.Level.DoorOpen.SafeCall();

        StartCoroutine(OpenDoors());
    }

    IEnumerator OpenDoors()
    {
        float timer = 0f;
        float perc = 0f;

        while (timer <= timeToOpen)
        {
            perc = timer / timeToOpen;
            leftDoor.localPosition = new Vector3(Mathf.Lerp(0,leftDoorDestination.localPosition.x, perc), 0, 0);
            rightDoor.localPosition = new Vector3(Mathf.Lerp(0, rightDoorDestination.localPosition.x, perc), 0, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        // incrment score
        LevelController.Instance.GetScoreBonus(bonusValue, bonusPosition.position);

        finished = true;

    }

}
