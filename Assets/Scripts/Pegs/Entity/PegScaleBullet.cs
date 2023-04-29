using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PegScaleBullet : MonoBehaviour
{
    private const float BULLET_SCALE_MODIFIER = 0.2f;

    [SerializeField] private FloatVariable bulletScale;

    private void OnCollisionEnter2D(Collision2D other)
    {
        bulletScale.SetValue(bulletScale.value + BULLET_SCALE_MODIFIER);
        Destroy(gameObject);
    }
}
