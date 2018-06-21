using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantProjectile : Enemy {

    public float speed;
    private Vector3 lookingDirection;

    protected override void Update()
    {
        base.Update();

        transform.position += lookingDirection * (speed * Time.deltaTime);
    }

    public void Init(Vector3 lookingDirection)
    {
        this.lookingDirection = lookingDirection;
    }
    
}
