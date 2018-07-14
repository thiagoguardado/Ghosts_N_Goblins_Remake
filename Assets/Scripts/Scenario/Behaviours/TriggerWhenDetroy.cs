using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWhenDetroy : MonoBehaviour {

    ObjectDestroyedTriggeredAction trigger;

    public void SetupTrigger(ObjectDestroyedTriggeredAction trigger)
    {
        this.trigger = trigger;
    }


    private void OnDestroy()
    {
        trigger.Trigger();
    }
}
