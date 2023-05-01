using UnityEngine;

public class PegMaxBullet : Peg
{
    private const int MAX_BULLET_INCREASE = 1;

    [SerializeField] private IntegerVariable maxBulletCount;

    public override void OnPlayerCollision()
    {
        maxBulletCount.SetValue(maxBulletCount.value + MAX_BULLET_INCREASE);
        MusicMixer.instance.PlayEffect(1);
        Destroy(gameObject);
    }
}