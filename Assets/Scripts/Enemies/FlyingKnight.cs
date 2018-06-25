using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingKnight : Enemy {

    [Header("FlyingKnight Knight Options")]
    public float horizontalSpeed = 1f;
    public float oscillationAmplitude = 2f;
    public float oscillationSpeed = 1f;
    private float initialPositionY;
    private float initialTime;


	protected override void Start()
	{
        base.Start();

        initialPositionY = transform.position.y;
        initialTime = Time.time;
	}

	protected override void Update()
	{
        base.Update();

        Move();
	}


    private void Move()
    {
        transform.position += Vector3.left * horizontalSpeed * Time.deltaTime;
        float positionY = initialPositionY - oscillationAmplitude * Mathf.Sin(2 * Mathf.PI * oscillationSpeed * (Time.time - initialTime));
        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
    }



}
