using UnityEngine;

public class PegGravityBullet : Peg
{
    private const float GRAVITY_SCALE_MODIFIER = -0.15f;

    [SerializeField] private FloatVariable gravityScale;

    public override void OnPlayerCollision()
    {
        gravityScale.SetValue(gravityScale.value + GRAVITY_SCALE_MODIFIER);
        Destroy(gameObject);
    }
}
