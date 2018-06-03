using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
public class PlayerController : MonoBehaviour {


    

    private void Awake()
    {
        // get references
        animationController = GetComponent<PlayerAnimationController>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }


    void Update () {


        CheckIfGrounded();

        // Get Inputs
        ResolveInputs();

	}

    private void CheckIfGrounded()
    {

        int layerMask = ~LayerMask.GetMask("Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundingRayLength, layerMask);
        Debug.DrawRay(transform.position, Vector3.down * groundingRayLength);

        if (hit.transform != null)
        {

            Debug.Log(hit.transform.name);

            if(hit.transform.gameObject.GetComponent<Floor>() != null)
            {

                if (!isGrounded && isJumping)
                {
                    isJumping = false;
                    animationController.isJumping = false;
                }

                isGrounded = true;
            }
        }
        else {
            isGrounded = false;
        }



    }

    private void ResolveInputs()
    {

        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");
        throwButton = Input.GetButtonDown("Fire1");
        jumpButton = Input.GetButtonDown("Jump");


        Crouch(verticalAxis);
        Walk(horizontalAxis);
        Jump(jumpButton);


    }


    // Crouch
    private void Crouch(float verticalAxis)
    {
        if()
    }


    // Jump
    private void Jump(bool jumpButton)
    {
        if (isGrounded && jumpButton)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce);
            animationController.isJumping = true;
            isJumping = true;
        }
    }




    // Walk
    private void Walk(float horizontalAxis)
    {

        if (horizontalAxis != 0 && !walkPaused)
        {
            // translate
            transform.Translate(Vector3.right * horizontalAxis * horizontalSpeed * Time.deltaTime);

            // turn
            if (horizontalAxis > 0 && currentDirection == Direction.Left)
            {
                // turn to right
                Vector3 scale = spriteTransform.localScale;
                scale.x = 1;
                spriteTransform.localScale = scale;
                currentDirection = Direction.Right;

            }
            else if (horizontalAxis < 0 && currentDirection == Direction.Right)
            {
                // turn to left
                Vector3 scale = spriteTransform.localScale;
                scale.x = -1;
                spriteTransform.localScale = scale;
                currentDirection = Direction.Left;
            }

            // update animation
            animationController.isRunning = true;
        }
        else
        {
            // update animation
            animationController.isRunning = false;
        }


       
    }


    // Jump

    // Ladder


}
 **/
