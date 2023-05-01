using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static Color ClearWhite = new Color(1, 1, 1, 0);

    public static void SetTimeout(MonoBehaviour mb, float delay, System.Action action)
    {
        mb.StartCoroutine(DelayCoroutine(delay, action));
    }

    static IEnumerator DelayCoroutine(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    public static void TweenColor(Image image, Color toColor, float duration = 1f)
    {
        LeanTween.value(image.gameObject, (c) =>
        {
            image.color = c;
        }, image.color, toColor, duration);

    }

    internal static void TweenColor(SpriteRenderer spriteRenderer, Color toColor, float duration = 1f)
    {
        LeanTween.value(spriteRenderer.gameObject, (c) =>
        {
            spriteRenderer.color = c;
        }, spriteRenderer.color, toColor, duration);
    }

    internal static void TweenColor(TMP_Text text, Color toColor, float duration = 1f)
    {
        LeanTween.value(text.gameObject, (c) =>
        {
            text.color = c;
        }, text.color, toColor, duration);
    }
}
