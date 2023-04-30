using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jerry : MonoBehaviour
{
    new SpriteRenderer renderer;
    [SerializeField]
    Sprite thinkingJerry;
    [SerializeField]
    Sprite neutralJerry;
    [SerializeField]
    Sprite talkingJerry;
    bool isNeutral;
    float timeSinceChange;
    float changeInterval = 10000f;
    Sprite actingSprite;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        actingSprite = neutralJerry;
    }

    public void Think()
    {
        actingSprite = thinkingJerry;
        changeInterval = 2f;
    }

    public void Talk()
    {
        actingSprite = talkingJerry;
        changeInterval = 0.5f;
    }

    public void Stop()
    {
        actingSprite = neutralJerry;
        changeInterval = 1000;
    }

    void Update()
    {
        timeSinceChange += Time.deltaTime;
        if (timeSinceChange > changeInterval)
        {
            renderer.sprite = isNeutral ? actingSprite : neutralJerry;
            isNeutral = !isNeutral;
            timeSinceChange = 0;
        }
    }
}
