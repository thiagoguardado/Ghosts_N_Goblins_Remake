using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("FloorCheck")]
    public Transform floorRayCheckStart;
    public float floorRayLenght;
    public bool hasFloorAhead { get; private set; }

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

    protected override void Update()
    {
        base.Update();

        CheckFloorAhead();
    }

    private void CheckFloorAhead()
    {
        hasFloorAhead = Physics2D.Raycast(floorRayCheckStart.position, Vector2.down, floorRayLenght, 1 << LayerMask.NameToLayer("Floor"));
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(floorRayCheckStart.position, Vector3.down * floorRayLenght);
    }
}
