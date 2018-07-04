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

    [Header("Unicorn Jumping")]
    public float jumpForce;
    public float minJumpDuration = 0.2f;
    public bool jump;

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

    public void Jump()
    {
        //rb.velocity = Vector2.up * jumpVelocity;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


    public void Ground(){
        rb.velocity = Vector2.zero;
    }

}
