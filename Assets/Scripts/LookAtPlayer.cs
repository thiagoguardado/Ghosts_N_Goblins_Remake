using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    public SpriteDirection spriteDirection;
 
    void Update () {

        TurnToPlayer();

    }

    private void TurnToPlayer()
    {

        spriteDirection.FaceDirection(LookToPlayerDirection(transform.position));
    }


    public static LookingDirection LookToPlayerDirection(Vector3 currentPosition)
    {
        if ((currentPosition.x - PlayerController.Instance.transform.position.x) > 0)
        {
            return LookingDirection.Left;
        }
        else
        {
            return LookingDirection.Right;
        }
    }
}
