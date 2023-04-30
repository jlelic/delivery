using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    new Collider2D collider;
    ParticleSystem particles;
    new SpriteRenderer renderer;
    new Rigidbody2D rigidbody;

    bool hit;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        particles = GetComponent<ParticleSystem>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.otherCollider.gameObject.name);
 
        if (hit)
        {
            return;
        }

        hit = true;
        rigidbody.gravityScale = 0.3f;
        particles.Play();
        Utils.TweenColor(renderer, Utils.ClearWhite, 3f);
        Destroy(gameObject, 3);
    }
}
