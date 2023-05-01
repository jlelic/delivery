using System;
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
    [SerializeField]
    TMP_Text creditsText;

    void Start()
    {
        introText.color = Color.white;
        titleText.color = new Color(0.76f, 0.2f, 0.2f); //red
        creditsText.color = Color.white;
        startGameButtonText.color = Color.black;
        startGameButton.color = Color.white;
        introOverlay.gameObject.SetActive(true);
    }


    public void OnStartGameClick()
    {
        Utils.TweenColor(introText, Utils.ClearWhite);
        Utils.TweenColor(titleText, Utils.ClearWhite);
        Utils.TweenColor(creditsText, Utils.ClearWhite);

        startGameButton.enabled = false;
        startGameButton.gameObject.SetActive(false);
        LeanTween.move(introOverlay.gameObject, introText.transform.position + new Vector3(7.5f, 0, 0), 1f)
            .setEase(LeanTweenType.easeInExpo);
        LeanTween.scale(introOverlay.gameObject, Vector3.one * 2.5f, 1f)
            .setEase(LeanTweenType.easeInExpo);
        Utils.SetTimeout(this, 0.5f, () =>
        {
            Utils.TweenColor(introOverlay, Color.clear, 0.5f);
        });
        GameManager.Instance.StartNewGame();
    }

    public void ShowOutro(int totalScore)
    {
        introText.text = GetResultString(totalScore) + "\n\nFinal score: " + totalScore;
        Utils.TweenColor(introText, Color.white, 3);
        Utils.TweenColor(titleText, new Color(0.76f, 0.2f, 0.2f)); // red
        Utils.TweenColor(creditsText, Color.white, 3);
        Utils.TweenColor(introOverlay, Color.black, 2f);
        LeanTween.move(introOverlay.gameObject, Vector3.zero, 2f)
            .setEase(LeanTweenType.easeOutExpo);
        LeanTween.scale(introOverlay.gameObject, Vector3.one, 2)
            .setEase(LeanTweenType.easeOutExpo);

        Utils.SetTimeout(this, 2f, () =>
        {
            startGameButton.gameObject.SetActive(true);
            startGameButton.enabled = true;
            startGameButtonText.text = "Play again";
            startGameButtonText.color = Color.black;
            Utils.TweenColor(startGameButton, Color.white);
        });

    }

    string GetResultString(int totalScore)
    {
        if (totalScore == 100)
        {
            return "Wow!!! You are a true comedy legend! No one has ever done that in the history of comedy. You deserve 4 oscars and a Netflix special!";
        }
        else if (totalScore >= 95 && totalScore < 100)
        {
            return "Phenomenal performance! The audience couldn't stop laughing, and you've just been offered your own comedy tour. Your future is looking bright!";
        }
        else if (totalScore >= 90 && totalScore < 95)
        {
            return "Incredible! You had the crowd rolling on the floor with laughter. You're definitely a rising star in the comedy world, and talent scouts are taking notice!";
        }
        else if (totalScore >= 85 && totalScore < 90)
        {
            return "Amazing show! Your clever punchlines and impeccable timing left the audience in stitches. You're well on your way to becoming a comedy sensation!";
        }
        else if (totalScore >= 80 && totalScore < 85)
        {
            return "Great job! Your jokes were on point, and you've gained a loyal fanbase. Keep it up, and you might just become a household name in comedy!";
        }
        else if (totalScore >= 75 && totalScore < 80)
        {
            return "Not bad at all! You had some solid laughs and a few groans, but overall, you held your own on stage. Keep honing your skills, and you'll be headlining in no time!";
        }
        else if (totalScore >= 70 && totalScore < 75)
        {
            return "You delivered a respectable performance with a mix of hits and misses. The audience enjoyed themselves, but there's still room to grow. Keep at it!";
        }
        else if (totalScore >= 65 && totalScore < 70)
        {
            return "You had your moments, but there's definitely room for improvement. Keep working on your material and stage presence, and you'll get better!";
        }
        else if (totalScore >= 60 && totalScore < 65)
        {
            return "An average performance with a few laughs sprinkled throughout. It's time to workshop some new jokes and up your comedy game!";
        }
        else if (totalScore >= 55 && totalScore < 60)
        {
            return "Well, you got a few chuckles here and there. Maybe comedy isn't your strong suit, but don't give up just yet! Keep learning and growing.";
        }
        else if (totalScore >= 50 && totalScore < 55)
        {
            return "You managed to get some polite laughter, but the audience was mostly unimpressed. Time to reevaluate your material and come back stronger!";
        }
        else if (totalScore >= 45 && totalScore < 50)
        {
            return "Yikes! That was a bit rough. But hey, even the best comedians have off nights. Keep practicing, and don't let one bad show bring you down!";
        }
        else if (totalScore >= 40 && totalScore < 45)
        {
            return "The silence in the room was deafening, but at least nobody booed you off stage. Time to hit the drawing board and come up with some fresh material.";
        }
        else if (totalScore >= 35 && totalScore < 40)
        {
            return "Ouch! The audience was not feeling your jokes tonight. You'll need to do some serious soul-searching and joke-writing before hitting the stage again.";
        }
        else if (totalScore >= 30 && totalScore < 35)
        {
            return "You bombed so hard, the club owner is considering installing a trap door on stage. Time to regroup and rethink your comedy strategy.";
        }
        else if (totalScore >= 25 && totalScore < 30)
        {
            return "Well, that was a disaster. The audience threw tomatoes at you, but hey, at least you got some free produce and a memorable story to tell!";
        }
        else if (totalScore >= 20 && totalScore < 25)
        {
            return "That was painful to watch. You might want to consider this career again... or at least take a comedy class or two before your next attempt.";
        }
        else if (totalScore >= 15 && totalScore < 20)
        {
            return "Rough night! Your jokes fell flat, and the audience was not amused. It's back to the drawing board for you, but don't give up on your dreams just yet!";
        }
        else if (totalScore >= 10 && totalScore < 15)
        {
            return "The audience was so unimpressed that they started telling their own jokes to pass the time. Time for some serious self-reflection and a new approach to comedy.";
        }
        else if (totalScore >= 5 && totalScore < 10)
        {
            return "Your performance was so bad that it's now being used as a cautionary tale for aspiring comedians. Better luck next time... if there is a next time.";
        }
        else if (totalScore > 0 && totalScore < 5)
        {
            return "You barely escaped with your life after the audience chased you off stage. Maybe it's time to rethink your comedy career and explore other talents.";
        }
        else // totalScore == 0
        {
            return "You had to run away from the comedy club after customers threatened your life and demanded refunds. The president has suggested that you're banned from every comedy club in the country by law.";
        }
    }

}
