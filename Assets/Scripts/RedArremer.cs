using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedArremer : Enemy
{

    [Header("Red Arremer Parameters")]
    public Animator animator;
    public float maxHeightWhenAscending;
    public float ascendingDescendingVerticalSpeed;
    public float ascendingDescendingHorizontalSpeed;
    public float movingOnGroundSpeed;
    public LookingDirection startWalkingDirection;

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
    public bool isGrounded { get; private set; }

    private Vector3 movingDirection;
    [HideInInspector] public bool isFlying;
    [HideInInspector] public bool isWalking;

    private float groundedTimer = 0f;

    protected override void Awake()
    {
        base.Awake();

        // configure grounding contact filter
        floorContactFilter = new ContactFilter2D();
        floorContactFilter.layerMask = 1 << LayerMask.NameToLayer("Floor");
        overlappingColliders = new Collider2D[5];

        // start moving direction
        switch (startWalkingDirection)
        {
            case LookingDirection.Left:
                movingDirection = Vector3.left;
                break;
            case LookingDirection.Right:
                movingDirection = Vector3.right;
                break;
            default:
                break;
        }
    }

    protected override void Update()
    {
        base.Update();

        CheckGround();

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

    private void CheckGround()
    {
        overlappingColliders = new Collider2D[5];
        enemyCollider.OverlapCollider(floorContactFilter, overlappingColliders);
        
        for (int i = 0; i < overlappingColliders.Length; i++)
        {
            if (overlappingColliders[i] == null)
            {
                break;
            }

            if (overlappingColliders[i].GetComponent<Floor>() != null)
            {
                isGrounded = true;
                return;
            }
        }

        isGrounded = false;
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
        MoveLinearly(movingDirection, movingOnGroundSpeed);
    }

    private void MoveLinearly(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;
    }



	public override void Hit(int damageTaken, Vector2 hitPoint)
	{
        base.Hit(damageTaken, hitPoint);

        animator.SetTrigger("Hurt");
	}

    public void Ground()
    {
        groundedTimer = 0f;
    }

    public void StartToAscend()
    {
        isFlying = true;
    }

    public void StartToDescend()
    {
        isFlying = false;
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
        if (UnityEngine.Random.Range(0, 1) > 0.5f)
        {
            movingDirection = Vector3.left;
        }
        else {
            movingDirection = Vector3.right;
        }
        movingDirection = movingDirection == Vector3.left ? Vector3.right : Vector3.left;
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
