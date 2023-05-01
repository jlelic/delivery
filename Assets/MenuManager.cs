using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text introText;
    [SerializeField]
    TMP_Text titleText;
    [SerializeField]
    TMP_Text startGameButtonText;
    [SerializeField]
    Image startGameButton;
    [SerializeField]
    SpriteRenderer introOverlay;

    void Start()
    {
        introText.color = Color.white;
        titleText.color = Color.red;
        startGameButtonText.color = Color.black;
        startGameButton.color = Color.white;
        introOverlay.gameObject.SetActive(true);
    }


    public void OnStartGameClick()
    {
        Utils.TweenColor(introText, Utils.ClearWhite);
        Utils.TweenColor(titleText, Utils.ClearWhite);
        Utils.TweenColor(startGameButtonText, Color.clear);
        Utils.TweenColor(startGameButton, Utils.ClearWhite);
        startGameButton.enabled = false;
        LeanTween.move(introOverlay.gameObject, introText.transform.position + new Vector3(7.5f, 0, 0), 1f)
            .setEase(LeanTweenType.easeInExpo);
        LeanTween.scale(introOverlay.gameObject, Vector3.one * 2.5f, 1f)
            .setEase(LeanTweenType.easeInExpo);
        Utils.SetTimeout(this, 0.5f, () =>
        {
            Utils.TweenColor(introOverlay, Color.clear, 0.5f);
        });
        Utils.SetTimeout(this, 3f, () =>
        {
            GameManager.Instance.LoadNextLevel();
        });
    }
}
