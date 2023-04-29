using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Collider2D))]
public class PegWall : MonoBehaviour
{
    private const int WALL_MAX_HITS = 3;

    [SerializeField] private List<Sprite> hitStateSprites;

    private SpriteRenderer spriteRenderer;
    private int hits = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = hitStateSprites[hits];
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            hits++;
            if (hits == WALL_MAX_HITS)
            {
                Destroy(gameObject);
            }
            else
            {
                spriteRenderer.sprite = hitStateSprites[hits];
            }
        }
    }
}
