using UnityEngine;

public class PegAmmo : Peg
{
    [SerializeField] private IntegerVariable bulletCount;

    public override void OnPlayerCollision()
    {
        bulletCount.SetValue(bulletCount.value + 1);
        Destroy(gameObject);
    }
}
