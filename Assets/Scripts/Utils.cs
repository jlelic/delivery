using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void SetTimeout(MonoBehaviour mb, float delay, System.Action action)
    {
        mb.StartCoroutine(DelayCoroutine(delay, action));
    }

    static IEnumerator DelayCoroutine(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
