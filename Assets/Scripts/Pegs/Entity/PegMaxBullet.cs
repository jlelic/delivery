using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PegMaxBullet : MonoBehaviour
{
    private const int MAX_BULLET_INCREASE = 1;

    [SerializeField] private IntegerVariable maxBulletModifier;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            maxBulletModifier.SetValue(maxBulletModifier.value + MAX_BULLET_INCREASE);
            Destroy(gameObject);
        }
    }
}