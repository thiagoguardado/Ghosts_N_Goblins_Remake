using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundedCheck))]
public class RedArremer : Enemy
{

    [Header("Red Arremer Parameters")]
    public Animator animator;
    public float maxHeightWhenAscending;
    public float ascendingDescendingVerticalSpeed;
    public float ascendingDescendingHorizontalSpeed;
    public float movingOnGroundSpeed;

    [Header("FSM Behavior")]
    public float distanceFromPlayerToStartMovement;
    public float timePreparingToFly;
    public float timeFlying;
    public float timeWalking;
    public float timeStopped;
    public float timeGrounded;
    private bool sawPlayer;

    // floor detection
    private ContactFilter2D floorContactFilter;
    private Collider2D[] overlappingColliders;
    public bool isGrounded { 
        get
        {
            return gc.isGrounded;
        }
    }

    [HideInInspector] public bool isFlying;
    [HideInInspector] public bool isWalking;

    private float groundedTimer = 0f;
    private GroundedCheck gc;

    protected override void Awake()
    {
        base.Awake();

        gc = GetComponent<GroundedCheck>();

        // configure grounding contact filter
        floorContactFilter = new ContactFilter2D();
        floorContactFilter.layerMask = 1 << LayerMask.NameToLayer("Floor");
        overlappingColliders = new Collider2D[5];

    }

    protected override void Update()
    {
        base.Update();

        if (!sawPlayer)
        {
            CheckIfSeePlayer();
        }

        if (isGrounded)
        {
            groundedTimer += Time.deltaTime;
            if (groundedTimer >= timeGrounded)
            {
                StartToAscend();
            }
        }

    }

    private void CheckIfSeePlayer()
    {
        if (Mathf.Abs(PlayerController.Instance.transform.position.x - transform.position.x) <= distanceFromPlayerToStartMovement)
        {
            sawPlayer = true;
            isFlying = true;
        }
    }

    public void Ascend()
    {
        MoveLinearly(Vector3.up, ascendingDescendingVerticalSpeed);

        MoveLinearly(PlayerDirection(), ascendingDescendingHorizontalSpeed);

    }

    public void Descend()
    {
        MoveLinearly(Vector3.down, ascendingDescendingVerticalSpeed);

        MoveLinearly(PlayerDirection(), ascendingDescendingHorizontalSpeed);
    }

    public void MoveOnGround()
    {
        MoveLinearly(spriteDirection.WorldLookingDirection, movingOnGroundSpeed);
    }

    private void MoveLinearly(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;
    }



	public override void Hit(int damageTaken, Vector2 hitPoint, LookingDirection hitDirection)
	{
        base.Hit(damageTaken, hitPoint, hitDirection);

        animator.SetTrigger("Hurt");
	}

    public void Ground()
    {
        return;
    }

    public void StartToAscend()
    {
        isFlying = true;
        groundedTimer = 0f;
    }

    public void StartToDescend()
    {
        isFlying = false;

        // notify event
        GameEvents.Enemies.RedArremerDive.SafeCall();
    }

    public void StartWalking()
    {
        isWalking = true;
    }

    public void Fly()
    {
        // move sin
        MoveLinearly(PlayerDirection(), ascendingDescendingHorizontalSpeed);
    }

    public void StopWalking()
    {
        isWalking = false;
    }

    public void ChangeWalkingDirection()
    {
        if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
        {
            spriteDirection.FaceDirection(LookingDirection.Left);
        }
        else {
            spriteDirection.FaceDirection(LookingDirection.Right);
        }

    }


    private Vector3 PlayerDirection()
    {
        if ((transform.position.x - PlayerController.Instance.transform.position.x) > 0)
        {
            return Vector3.left;
        }
        else
        {
            return Vector3.right;
        }
    }
}
