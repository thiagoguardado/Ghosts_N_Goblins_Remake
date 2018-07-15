using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectScore))]
public class StaticCollectible : MonoBehaviour, IPlayerTouchable {

    public Transform spriteTransform;

    public void WasTouchedByPlayer()
    {

        // add score to game controller
        GetComponent<ObjectScore>().IncrementGameScore(spriteTransform.GetComponent<Collider2D>().bounds.center);

        // notify event
        GameEvents.Player.PlayerPickedTreasure.SafeCall();

        // destroy object
        Destroy(gameObject);

    }

}
