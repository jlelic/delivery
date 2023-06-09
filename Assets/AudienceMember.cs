using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudienceMemberPosition
{
    Back = 0,
    Right = 1
}

enum AnimationStartFrame
{
    Laugh = 0,
    Bored = 2,
    Anger = 4,
    Neutral = 6,
    Giggle = 8
}

public class AudienceMember : MonoBehaviour
{
    new SpriteRenderer renderer;
    public Sprite[] spriteSheet;

    [SerializeField]
    Transform projectileSource;

    public AudienceMemberPosition position;

    int animationFrame;
    int animationStartFrame;
    float currentFrameTime;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Neutral()
    {
        switch((AnimationStartFrame)animationStartFrame)
        {
            case AnimationStartFrame.Laugh:
                animationStartFrame = (int)AnimationStartFrame.Giggle;
                break;
            case AnimationStartFrame.Bored:
                animationStartFrame = (int)AnimationStartFrame.Bored;
                break;
            default:
                animationStartFrame = (int)AnimationStartFrame.Neutral;
                break;
        }
    }

    public void React(int score)
    {
        var newAnimation = AnimationStartFrame.Neutral;
        var randBool = UnityEngine.Random.value > 0.5;
        if (score == 0)
        {
            newAnimation = AnimationStartFrame.Anger;
        }
        else if (score <= 3)
        {
            newAnimation = randBool ? AnimationStartFrame.Bored : AnimationStartFrame.Anger;
        }
        else if (score < 5)
        {
            newAnimation = randBool ? AnimationStartFrame.Bored : AnimationStartFrame.Neutral;
        }
        else if (score < 6)
        {
            newAnimation = randBool ? AnimationStartFrame.Giggle : AnimationStartFrame.Neutral;
        }
        else if (score < 8)
        {
            newAnimation = randBool ? AnimationStartFrame.Giggle : AnimationStartFrame.Laugh;
        }
        else
        {
            newAnimation = AnimationStartFrame.Laugh;
        }

        if (newAnimation == AnimationStartFrame.Anger && projectileSource != null)
        {
            var tomato = Instantiate(GameManager.Instance.tomato, projectileSource.position, Quaternion.identity);
            var tomatoRigidBody = tomato.GetComponent<Rigidbody2D>();
            tomatoRigidBody.gravityScale = 0;
            Utils.SetTimeout(this, UnityEngine.Random.value, () => {
                tomatoRigidBody.gravityScale = 1;
                tomatoRigidBody.AddForce(new Vector2(projectileSource.up.x, projectileSource.up.y) * (400 + (UnityEngine.Random.value*80)));
            }); 
        }

        animationStartFrame = (int)newAnimation;
    }

    private void Update()
    {
        currentFrameTime += Time.deltaTime;

        if(currentFrameTime > 0.5f + 0.2f*UnityEngine.Random.value)
        {
            animationFrame = 1 - animationFrame;
            renderer.sprite = spriteSheet[animationStartFrame + animationFrame];
            currentFrameTime = 0;
        }
    }
}
