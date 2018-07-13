using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bird : Enemy
{

    [Header("Bird Flight STart")]
    public float distanceToPlayerToStartFlying;
    public float waitBeforeStartMovingAfterStartFlying;

    [Header("Bird Movement")]
    public float speed;
    public float oscillationAmplitude;
    public float osclilationFrequency;


    [Header("Animation")]
    public Animator animator;

    public bool isFlying { get; private set; }
    private bool isMoving = false;
    private float startHeight;
    private float startFlyingTime;

    public bool startFlying = false;


    protected override void Update()
    {
        base.Update();

        if ((transform.position.x - PlayerController.Instance.transform.position.x) <= distanceToPlayerToStartFlying && !isFlying)
        {
            StartFlying();
        }

    }

    private void StartFlying() {

        isFlying = true;

        // notify event
        GameEvents.Enemies.CrowStartedToFly.SafeCall();

    }

    public void Move()
    {

        if (!isMoving)
        {
            isMoving = true;
            startFlyingTime = Time.time;
            startHeight = transform.position.y;
        }

        transform.position = new Vector3(transform.position.x + spriteDirection.WorldLookingDirection.x * speed * Time.deltaTime, startHeight + (Mathf.Sin((Time.time - startFlyingTime ) * osclilationFrequency - Mathf.PI/2) + 1 ) * oscillationAmplitude, transform.position.z);

    }

}
