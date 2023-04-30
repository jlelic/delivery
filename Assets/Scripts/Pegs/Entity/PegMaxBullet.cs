using UnityEngine;

public class PegMaxBullet : Peg
{
    private const int MAX_BULLET_INCREASE = 1;

    [SerializeField] private IntegerVariable maxBulletModifier;

    public override void OnPlayerCollision()
    {
        maxBulletModifier.SetValue(maxBulletModifier.value + MAX_BULLET_INCREASE);
        Destroy(gameObject);
    }
}