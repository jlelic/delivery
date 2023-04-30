using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PegWall : Peg
{
    private const int WALL_MAX_HITS = 3;

    [SerializeField] private List<Sprite> hitStateSprites;

    private SpriteRenderer spriteRenderer;
    private int hits = 0;

    public override void OnPlayerCollision()
    {
        hits++;
        if (hits == WALL_MAX_HITS)
        {
            TriggerParticleEfect();
            Destroy(gameObject);
        }
        else
        {
            spriteRenderer.sprite = hitStateSprites[hits];
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = hitStateSprites[hits];
    }
}
