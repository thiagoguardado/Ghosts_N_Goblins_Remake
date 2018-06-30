using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GroundedCheck))]
public class Pot : MonoBehaviour {

    private GroundedCheck groundedCheck;
    private Rigidbody2D rb;
    public GameObject potItem;
    private bool isHeld = true;
    public bool release = false;

    private void Awake()
    {
        groundedCheck = GetComponent<GroundedCheck>();
        rb = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update () {

        if (release && isHeld)
        {
            Release();
        }

        if (!isHeld)
        {
            if (groundedCheck.isGrounded)
            {
                BreakPot();
            }
        }
	}

    public void Release()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        isHeld = false;
    }

    private void BreakPot()
    {
        Instantiate(potItem, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
