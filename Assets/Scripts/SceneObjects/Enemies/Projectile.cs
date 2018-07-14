using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CheckEnemyOverlapCollision))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {

    public float speed;
    private Vector3 moveDirection = Vector3.zero;
    public SpriteDirection spriteDirection;
    public int damageOnHit;
    public bool destroyWhenHit = false;

    private CheckEnemyOverlapCollision checkCollision;

    public void Init(Vector3 direction, LookingDirection lookingDirection)
    {
        moveDirection = direction;
        spriteDirection.FaceDirection(lookingDirection);
    }

    private void Awake()
    {
        checkCollision = GetComponent<CheckEnemyOverlapCollision>();

        checkCollision.HitSomething += HitSomething;
    }

    private void OnDestroy()
    {
        checkCollision.HitSomething -= HitSomething;
    }

    private void Update () {

        transform.position += moveDirection * (speed * Time.deltaTime);
    }

    private void HitSomething(IEnemyHittable hitObject)
    {
        hitObject.Hit(damageOnHit, transform.position);

        if (destroyWhenHit)
            Destroy(gameObject);
    }



}
