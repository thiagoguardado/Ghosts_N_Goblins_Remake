using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonAwakeGeneric<T> : MonoBehaviour
        where T : SingletonAwakeGeneric<T>
{
    public static T Instance
    {
        get; private set;
    }

    protected virtual void Awake()
    {
        Instance = (T)this;
    }

}
