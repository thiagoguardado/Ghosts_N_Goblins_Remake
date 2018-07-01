using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantProjectile : Enemy {

    public float speed;
    public Vector3 direction;

    protected override void Update()
    {
        base.Update();

        transform.position += direction * (speed * Time.deltaTime);
    }

    public void Init(Vector2 shootDirection)
    {
        direction = shootDirection;
    }
    
}
