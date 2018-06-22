using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedArremer : Enemy
{

    [Header("Red Arremer Parameters")]
    public Animator animator;
    public bool isFlying;
    public bool isWalking;
    public float maxHeightWhenAscending;
    public float timePreparingToFly;
    public float ascendingDescendingSpeed;
    public bool isGrounded {get; private set;}
    public float raycastLenght = 0.01f;

    public void Ascend()
    {
        MoveLinearly(Vector3.up, ascendingDescendingSpeed);
    }

    public void Descend()
    {
        MoveLinearly(Vector3.down, ascendingDescendingSpeed);
    }

    private void MoveLinearly(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;
    }

	protected override void Update()
	{
        base.Update();

        // check ground
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, raycastLenght, 1 << LayerMask.NameToLayer("Floor"));
        Debug.DrawRay(transform.position, Vector2.down * raycastLenght);
        if(hitInfo.collider !=null){
            isGrounded = true;
        } else {
            isGrounded = false;
        }
	}

	public override void Hit(int damageTaken, Vector2 hitPoint)
	{
        base.Hit(damageTaken, hitPoint);

        animator.SetTrigger("Hurt");
	}
}
