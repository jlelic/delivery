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

    [SerializeField]
    Transform projectileSource;

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
        if (score == 0)
        {
            newSprite = angryPose;
        }
        else if (score <= 3)
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
        }
        else
        {
            newSprite = laughingPose;
        }

        if (newSprite == angryPose && projectileSource != null)
        {
            var tomato = Instantiate(GameManager.Instance.tomato, projectileSource.position, Quaternion.identity);
            var tomatoRigidBody = tomato.GetComponent<Rigidbody2D>();
            tomatoRigidBody.gravityScale = 0;
            Utils.SetTimeout(this, UnityEngine.Random.value, () => {
                tomatoRigidBody.gravityScale = 1;
                tomatoRigidBody.AddForce(new Vector2(projectileSource.up.x, projectileSource.up.y) * (400 + (UnityEngine.Random.value*80)));
            }); 
        }

        renderer.sprite = newSprite;
    }
}
