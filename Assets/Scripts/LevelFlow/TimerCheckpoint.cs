using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCheckpoint : Checkpoint
{

    public int newTimer = 60;

    protected override void CheckpointReached()
    {
        LevelController.Instance.ExtendTime(newTimer);
    }
}
