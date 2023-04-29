using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PegAmmo : MonoBehaviour
{
    [SerializeField] private IntegerVariable bulletCount;

    private void OnCollisionEnter2D(Collision2D other)
    {
        bulletCount.SetValue(bulletCount.value + 1);
        Destroy(gameObject);
    }
}
