using System;
using System.Collections;
using UnityEngine;

public static class MyExtensions
{

    public static Vector2 FromVector3(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }

    public static void SafeCall(this Action action)
    {
        if (action != null)
        {
            action();
        }
    }

    public static void WaitAndAct(this MonoBehaviour mono, float wait, Action action)
    {
        mono.StartCoroutine(WaitBeforeAct(wait,action));
    }

    private static IEnumerator WaitBeforeAct(float wait, Action action)
    {
        yield return new WaitForSeconds(wait);
        action();
    }


}
