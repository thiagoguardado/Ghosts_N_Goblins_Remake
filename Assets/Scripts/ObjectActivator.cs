using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{

    void Awake()
    {
        CameraController.Instance.AddObjectToList(this);
    }

    void OnDestroy()
    {
        CameraController.Instance.RemoveObjctFromList(this);
    }

}
