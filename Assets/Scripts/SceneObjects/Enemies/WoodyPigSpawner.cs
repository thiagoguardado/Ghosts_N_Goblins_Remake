using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodyPigSpawner : EnemySpawner {

    public Collider2D spawnableArea;

    float minColliderX;
    float maxColliderX;
    float minColliderY;
    float maxColliderY;

    void Awake()
    {
        minColliderX = (spawnableArea.bounds.center - spawnableArea.bounds.extents).x;
        maxColliderX = (spawnableArea.bounds.center + spawnableArea.bounds.extents).x;
        minColliderY = (spawnableArea.bounds.center - spawnableArea.bounds.extents).y;
        maxColliderY = (spawnableArea.bounds.center + spawnableArea.bounds.extents).y;
    }

    protected override Vector3 SelectPointToSpawn()
    {
        float minX = Mathf.Clamp(CameraController.Instance.cameraBounds.BoundsWithOffset.xMin, minColliderX, maxColliderX);
        float maxX = Mathf.Clamp(CameraController.Instance.cameraBounds.BoundsWithOffset.xMax, minColliderX, maxColliderX);

        float minY = Mathf.Clamp(CameraController.Instance.cameraBounds.BoundsWithOffset.yMin, minColliderY, maxColliderY);
        float maxY = Mathf.Clamp(CameraController.Instance.cameraBounds.BoundsWithOffset.yMax, minColliderY, maxColliderY);

        return new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), transform.position.z);

    }


}
