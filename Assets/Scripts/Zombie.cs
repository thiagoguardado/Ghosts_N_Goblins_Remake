using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy, IWeaponHittable
{
    [Header("Movement")]
    public LookingDirection lookingDirection;
    public float speed;
    public Transform spriteTransform;

    [Header("Animation")]
    public Animator animator;

    [Header("FSM")]
    public float timeEmerging;
    public float timeWalking;
    public float timeSinking;


    public void Move()
    {
        transform.Translate((lookingDirection == LookingDirection.Left ? new Vector3(-1,0,0) : new Vector3(1,0,0)) * speed * Time.deltaTime);
    }

    public void StartSink()
    {
        animator.SetTrigger("Sink");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();

        switch (lookingDirection)
        {
            case LookingDirection.Left:
                spriteTransform.localScale = new Vector3(1, 1, 1);
                break;
            case LookingDirection.Right:
                spriteTransform.localScale = new Vector3(-1, 1, 1);
                break;
        }

    }

}
