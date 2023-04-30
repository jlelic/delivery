using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Peg : MonoBehaviour
{
    public abstract void OnPlayerCollision();

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnPlayerCollision();
        }
    }
}
