using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform playerTransform;

    private Vector3 distanceAtStart;


    private void Start()
    {
        distanceAtStart = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void Update () {


        transform.position = playerTransform.position + distanceAtStart;

	}
}
