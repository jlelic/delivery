using UnityEngine;

public class PegGravityBullet : MonoBehaviour
{
    private const float GRAVITY_SCALE_MODIFIER = -0.15f;

    [SerializeField] private FloatVariable gravityScale;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gravityScale.SetValue(gravityScale.value + GRAVITY_SCALE_MODIFIER);
            Destroy(gameObject);
        }
    }
}
