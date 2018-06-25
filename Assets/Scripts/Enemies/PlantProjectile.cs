using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantProjectile : Enemy {

    public float speed;

    protected override void Update()
    {
        base.Update();

        transform.position += spriteDirection.WorldLookingDirection * (speed * Time.deltaTime);
    }

    public void Init(LookingDirection lookingDirection)
    {
        spriteDirection.FaceDirection(lookingDirection);
    }
    
}
