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
        LeanTween.move(introOverlay.gameObject, introText.transform.position + new Vector3(10, 2, 0), 2f)
            .setEase(LeanTweenType.easeInExpo);
        LeanTween.scale(introOverlay.gameObject, Vector3.one * 2.5f, 2)
            .setEase(LeanTweenType.easeInExpo);
        Utils.SetTimeout(this, 1.4f, () =>
        {
            Utils.TweenColor(introOverlay, Color.clear, 0.6f);
        });
        Utils.SetTimeout(this, 3f, () =>
        {
            GameManager.Instance.LoadNextLevel();
        });
    }
}
