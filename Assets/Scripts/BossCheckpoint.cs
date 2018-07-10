using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCheckpoint : Checkpoint
{
    protected override void CheckpointReached()
    {
        GameEvents.Level.BossReached.SafeCall();
    }
}
