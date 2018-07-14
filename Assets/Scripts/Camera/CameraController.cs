using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonAwakeGeneric<CameraController> {

    public CameraBounds cameraBounds;

    private List<ObjectActivator> sceneActivatableObjects = new List<ObjectActivator>();

    void Update()
    {
        CheckObjectsInBounds();
    }

    public void AddObjectToList(ObjectActivator obj)
    {
        sceneActivatableObjects.Add(obj);
    }

    public void RemoveObjctFromList(ObjectActivator obj)
    {
        sceneActivatableObjects.Remove(obj);
    }

    public void CheckObjectsInBounds()
    {
        for (int i = 0; i < sceneActivatableObjects.Count; i++)
        {
            if (cameraBounds.BoundsWithOffset.Contains(sceneActivatableObjects[i].transform.position))
            {
                sceneActivatableObjects[i].SendActivationOrder();
            }
            else {
                sceneActivatableObjects[i].SendDeactivationOrder();
            }
        }
    }

}
