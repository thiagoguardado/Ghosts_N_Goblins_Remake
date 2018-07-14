using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pot : MonoBehaviour {

    private bool isGrounded;
    private Rigidbody2D rb;
    public GameObject potItem;
    private bool isHeld = true;
    public bool release = false;

    private void Awake()
    {
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
            if (isGrounded)
            {
                BreakPot();
            }
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    public void Release()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        isHeld = false;
    }

    private void BreakPot()
    {
        GameObject dropItem = DroppablesController.Instance.GetNextItem();

        Instantiate(dropItem, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
