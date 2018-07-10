using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Checkpoint : MonoBehaviour {

    string playerLayer = "Player";

    protected abstract void CheckpointReached();

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.attachedRigidbody.gameObject.layer == LayerMask.NameToLayer(playerLayer))
        {
            CheckpointReached();

            gameObject.SetActive(false);
        }

    }

}
