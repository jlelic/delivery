using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PegKey : MonoBehaviour
{
    [SerializeField]
    Image keyImage;
    [SerializeField]
    Image letterImage;

    Color semiClear = new Color(1, 1, 1, 0.5f);

    public bool isActive { get; private set; }

    private void Awake()
    {
        keyImage = GetComponent<Image>();
        letterImage = transform.GetChild(0).GetComponent<Image>();
        keyImage.color = semiClear;
        letterImage.color = Color.clear;

    }

    public void Activate()
    {
        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.scale(gameObject, Vector3.one * 1.5f, 0.2f).setEase(LeanTweenType.easeInCubic));
        sequence.append(LeanTween.scale(gameObject, Vector3.one, 0.2f).setEase(LeanTweenType.easeOutCubic));

        Utils.TweenColor(letterImage, Color.black, 0.4f);
        Utils.TweenColor(keyImage, Color.white, 0.2f);
        isActive = true;
    }

    public void Deactivate()
    {
        Utils.TweenColor(letterImage, Color.clear);
        Utils.TweenColor(keyImage, semiClear);
        isActive = false;
    }

    public void ShowFeedback()
    {

        if (!isActive)
        {
            MusicMixer.instance.PlayEffect(0, 1);
            transform.localScale = Vector3.one;
            LTSeq sequence = LeanTween.sequence();
            sequence.append(LeanTween.scale(gameObject, Vector3.one * 1.5f, 0.1f).setEase(LeanTweenType.easeInCubic));
            sequence.append(LeanTween.scale(gameObject, Vector3.one, 0.1f).setEase(LeanTweenType.easeOutCubic));
            letterImage.color = Color.black;
            Utils.TweenColor(keyImage, Color.red, 0.05f);
            Utils.SetTimeout(this, 0.15f, () =>
            {
                keyImage.color = semiClear;
                letterImage.color = Color.clear;
            });
        }
    }
}
