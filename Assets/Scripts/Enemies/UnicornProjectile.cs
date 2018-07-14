using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornProjectile : Enemy
{

    public float speed;
    public Vector3 direction;

    override protected void Start()
    {
        base.Start();

        //spriteDirection.FaceDirection(LookAtPlayer);
    }

    protected override void Update()
    {
        base.Update();

        transform.position += spriteDirection.WorldLookingDirection * (speed * Time.deltaTime);
    }

    public void Init(Vector2 shootDirection)
    {
        direction = shootDirection;
    }

}

