using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThreshold : MonoBehaviour {

    bool activated = false;

    void OnTriggerEnter2D(Collider2D other)
    {

        PlayerController pc = other.attachedRigidbody.GetComponent<PlayerController>();
        if (pc != null && !activated)
        {
            pc.Fall();
            activated = true;

        }


    }
}
