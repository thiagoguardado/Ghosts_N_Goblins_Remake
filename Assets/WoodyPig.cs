using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodyPig : Enemy {


    private LookingDirection lookingDirection = LookingDirection.Left;

    [Header("Woody Pig Config")]
    public float horizontalSpeed = 0.5f;


    public void Move()
    {
        transform.position += (lookingDirection == LookingDirection.Left ? Vector3.left : Vector3.right) * horizontalSpeed * Time.deltaTime;
    }

}
