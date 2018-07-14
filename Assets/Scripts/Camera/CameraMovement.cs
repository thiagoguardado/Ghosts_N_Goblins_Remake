using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform playerTransform;

    public Transform leftLimit;
    public Transform rightLimit;



    // Update is called once per frame
    void Update () {

        float xPosition = playerTransform.position.x;
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(xPosition, leftLimit.position.x, rightLimit.position.x);
        transform.position = position;

	}
}
