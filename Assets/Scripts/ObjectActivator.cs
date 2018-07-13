using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{

    bool isActive = true;

    void Awake()
    {
        CameraController.Instance.AddObjectToList(this);
    }

    void OnDestroy()
    {
        Deactivate();
        CameraController.Instance.RemoveObjctFromList(this);
    }

    public void SendActivationOrder()
    {
        if (!isActive)
        {
            isActive = true;
            Activate();
        }
    }

    public void SendDeactivationOrder()
    {
        if (isActive)
        {
            isActive = false;
            Deactivate();
        }
    }

    protected virtual void Activate()
    {
     
        gameObject.SetActive(true);
       
    }

    protected virtual void Deactivate()
    {

        gameObject.SetActive(false);
    }
}
