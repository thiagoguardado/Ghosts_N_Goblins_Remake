using System;
using System.Collections;
using UnityEngine;

public static class MyExtensions
{
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
