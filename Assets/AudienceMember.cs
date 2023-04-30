using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceMember : MonoBehaviour
{
    new SpriteRenderer renderer;
    [SerializeField]
    Sprite neutralPose;
    [SerializeField]
    Sprite boredPose;
    [SerializeField]
    Sprite smirkPose;
    [SerializeField]
    Sprite angryPose;
    [SerializeField]
    Sprite laughingPose;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Neutral()
    {
        renderer.sprite = neutralPose;
    }

    public void React(int score)
    {
        var newSprite = neutralPose;
        var randBool = UnityEngine.Random.value > 0.5;

        if (score <= 3)
        {
            newSprite = randBool ? boredPose : angryPose;
        }
        else if (score < 5)
        {
            newSprite = neutralPose;
        }
        else if (score < 6)
        {
            newSprite = randBool ? smirkPose : neutralPose;
        }
        else if (score < 8)
        {
            newSprite = randBool ? smirkPose : laughingPose;
        } else
        {
            newSprite = laughingPose;
        }

        renderer.sprite = newSprite;
    }
}
