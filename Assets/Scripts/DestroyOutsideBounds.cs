using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutsideBounds : MonoBehaviour {

    private void Update(){

        if (!CameraController.Instance.cameraBounds.BoundsWithOffset.Contains(transform.position))
        {
            Destroy(gameObject);
        }

    }
}
