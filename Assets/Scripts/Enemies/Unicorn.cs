using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundedCheck))]
public class Unicorn : Enemy {

    private Rigidbody2D rb;
    private GroundedCheck gc;

    [Header("Unicorn Grounded")]
    public float walkingSpeed;
    public float timeGroundedBeforeJump;
    public bool isGrounded { 
        get
        {
            return gc.isGrounded;
        }
    }
    public float timeGroundedToShoot;
    public Transform shootPosition;
    public UnicornProjectile projectile;

    [Header("Unicorn Jumping")]
    public float jumpForce;
    public float diagonalForceHorizontalDistanceFactor;
    public float minJumpDuration = 0.2f;
    public bool jump;

    [Header("Dashing")]
    public float dashSpeedMultiplier;
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
        gc = GetComponent<GroundedCheck>();
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
        transform.Translate(spriteDirection.WorldLookingDirection * walkingSpeed * dashSpeedMultiplier * Time.deltaTime);
        animator.SetBool("isWalking", true);
        animator.SetFloat("walkingSpeed", dashSpeedMultiplier);
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void JumpDiagonal(float distance)
    {
        Vector2 dir = new Vector2(diagonalForceHorizontalDistanceFactor * distance * spriteDirection.WorldLookingDirection.x, 1);
        rb.AddForce(dir * jumpForce, ForceMode2D.Impulse);
    }


    public void Ground(){
        rb.velocity = Vector2.zero;
    }

    protected override void HitSomething()
    {
        hitSomething = true;
    }

    public void ResetHitSomethingFlag()
    {
        hitSomething = false;
    }


    public void Shoot()
    {

        // shoot projectile
        Instantiate(projectile, shootPosition.position, projectile.transform.rotation);

    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, minDistanceToDahsAndJump);
    }

}
