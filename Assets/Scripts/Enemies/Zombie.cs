using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FloorChecker))]
public class Zombie : Enemy
{
    [Header("Movement")]
    public float speed;
    public Transform spriteTransform;

    [Header("Animation")]
    public Animator animator;

    [Header("FSM")]
    public float timeEmerging;
    public float timeWalking;
    public float timeSinking;

    public FloorChecker floorChecker { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        floorChecker = GetComponent<FloorChecker>();
    }


    protected override void Start()
    {
        base.Start();

        // Start Looking To Player
        spriteDirection.FaceDirection(LookAtPlayer.LookToPlayerDirection(transform.position));

        // notify
        GameEvents.Enemies.ZombieSpawned.SafeCall();
    }


    public void Move()
    {
        transform.Translate((spriteDirection.WorldLookingDirection) * speed * Time.deltaTime);
    }

    public void StartSink()
    {
        animator.SetTrigger("Sink");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    

}
