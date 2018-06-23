using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    public Transform spriteObject;
    public bool startLookingLeft;

    private Vector3 normalScale = new Vector3(1, 1, 1);
    private Vector3 flippedScale = new Vector3(-1, 1, 1);


    void Update () {

        TurnToPlayer();

    }

    private void TurnToPlayer()
    {
        if ((transform.position.x - PlayerController.Instance.transform.position.x) > 0 && startLookingLeft)
        {
            spriteObject.transform.localScale = normalScale;
        }
        else
        {
            spriteObject.transform.localScale = flippedScale;
        }
    }
}
