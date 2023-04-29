using UnityEngine;
using UnityEngine.Events;

public class PegDestroyer : MonoBehaviour
{
    [SerializeField] private UnityEvent bulletDestroyedEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PegBullet peg;
        if (other.gameObject.TryGetComponent<PegBullet>(out peg))
        {
            Destroy(peg.gameObject);
            bulletDestroyedEvent.Invoke();
        }
    }
}
