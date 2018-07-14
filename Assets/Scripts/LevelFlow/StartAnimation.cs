using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour {

    public bool finished = false;

    public void PickupArmor()
    {
        GameEvents.Player.PlayerPickedArmor.SafeCall();
    }

    public void Finish()
    {
        finished = true;
    }
}
