using UnityEngine;

public class PegScaleBullet : Peg
{
    private const float BULLET_SCALE_MODIFIER = 0.2f;

    [SerializeField] private FloatVariable bulletScale;

    public override void OnPlayerCollision()
    {
        bulletScale.SetValue(bulletScale.value + BULLET_SCALE_MODIFIER);
        MusicMixer.instance.PlayEffect(1);
        Destroy(gameObject);
    }
}
