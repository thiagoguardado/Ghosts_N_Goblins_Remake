using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingKnightObjectActivator : ObjectActivator {

    protected override void Activate()
    {
        base.Activate();

        GameEvents.Enemies.TriggerFlyingKnightEnter();
    }

    protected override void Deactivate()
    {
        base.Deactivate();

        GameEvents.Enemies.TriggerFlyingKnightLeft();
    }
}

