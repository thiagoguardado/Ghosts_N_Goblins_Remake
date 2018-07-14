using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundedCheck))]
public class Unicorn : Enemy {

    private Rigidbody2D rb;
    public GroundedCheck groundedCheck { get; private set; }

    [Header("Unicorn Grounded")]
    public float walkingSpeed;
    public float timeGroundedBeforeJump;
    public bool isGrounded { 
        get
        {
            return groundedCheck.isGrounded;
        }
    }

    [Header("Shot")]
    public float timeGroundedToShoot;
    public Transform shootPosition;
    public Projectile projectile;
    public float shotProbability;

    [Header("Unicorn Jumping")]
    public float jumpForce;
    public float diagonalForceHorizontalDistanceFactor;
    public float minJumpDuration = 0.2f;
    public bool jump;
    public float gravityForce;

    [Header("Dashing")]
    public float dashSpeed;
    public float dashAnimationSpeedMultiplier;
    public float dashMaxDuration;
    public bool hitSomething { get; private set; }
    public float minDistanceToDahsAndJump;

    [Header("References")]
    public Transform scoreAnchor;
    public Animator animator;

    protected override void Awake()
	{
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        groundedCheck = GetComponent<GroundedCheck>();
	}

	protected override void Update()
	{
        base.Update();

        if(jump)
        {
            jump = false;
            Jump();
        }
	}


	protected override void IncrementScore()
	{
        // add score to game controller
        GetComponent<ObjectScore>().IncrementGameScore(scoreAnchor.position);
	}

    public void Walk()
    {
        transform.Translate(spriteDirection.WorldLookingDirection * walkingSpeed * Time.deltaTime);
        animator.SetBool("isWalking", true);
    }

    public void Dash()
    {
        transform.Translate(spriteDirection.WorldLookingDirection * dashSpeed * Time.deltaTime);
        animator.SetBool("isWalking", true);
        animator.SetFloat("walkingSpeed", dashAnimationSpeedMultiplier);
    }

    public void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
        //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void JumpDiagonal(float distance)
    {
        Vector2 dir = new Vector2(diagonalForceHorizontalDistanceFactor * distance * spriteDirection.WorldLookingDirection.x, 1);
        rb.velocity = dir * jumpForce;
        //rb.AddForce(dir * jumpForce, ForceMode2D.Impulse);
    }

    public void AddGravity()
    {
        rb.velocity += Vector2.down * gravityForce * Time.deltaTime;
    }

    public void Ground(Vector2 groundingPoint)
    {
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(groundingPoint.x, groundingPoint.y, transform.position.z);
    }

    protected override void HitSomething(IEnemyHittable objectHit)
    {
        base.HitSomething(objectHit);

        hitSomething = true;
    }

    public void ResetHitSomethingFlag()
    {
        hitSomething = false;
    }


    public void Shoot()
    {
        // shoot projectile
        Projectile proj = Instantiate(projectile, shootPosition.position, projectile.transform.rotation);
        proj.Init(spriteDirection.WorldLookingDirection, spriteDirection.lookingDirection);
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, minDistanceToDahsAndJump);
    }

}
