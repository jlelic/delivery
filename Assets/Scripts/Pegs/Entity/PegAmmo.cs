using UnityEngine;

public class PegAmmo : Peg
{
    [SerializeField] private IntegerVariable bulletCount;

    public override void OnPlayerCollision()
    {
        MusicMixer.instance.PlayEffect(1);
        bulletCount.SetValue(bulletCount.value + 1);
        Destroy(gameObject);
    }
}
