using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : EnemySpawner
{

    float minColliderX;
    float maxColliderX;

    void Awake()
    {
        minColliderX = (thisCollider.bounds.center - thisCollider.bounds.extents).x;
        maxColliderX = (thisCollider.bounds.center + thisCollider.bounds.extents).x;
    }

    protected override Vector3 SelectPointToSpawn()
    {

        float minX = Mathf.Clamp(CameraController.Instance.cameraBounds.BoundsWithOffset.xMin, minColliderX, maxColliderX);
        float maxX = Mathf.Clamp(CameraController.Instance.cameraBounds.BoundsWithOffset.xMax, minColliderX, maxColliderX);
        return new Vector3( Random.Range(minX, maxX), transform.position.y, transform.position.z);
        
    }


}
