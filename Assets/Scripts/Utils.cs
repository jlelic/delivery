using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public static void TweenColor(Image image, Color toColor, float duration = 1f, float delay = 1f)
    {
        LeanTween.value(image.gameObject, (c) =>
        {
            Debug.Log(c);
            image.color = c;
        }, image.color, toColor, duration);

    }

    internal static void TweenColor(TMP_Text text, Color toColor, float duration = 1f, float delay = 1f)
    {
        LeanTween.value(text.gameObject, (c) =>
        {
            Debug.Log(c);
            text.color = c;
        }, text.color, toColor, duration);
    }
}
