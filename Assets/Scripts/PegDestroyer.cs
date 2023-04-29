using UnityEngine;

public class PegDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PegBall peg;
        if (other.gameObject.TryGetComponent<PegBall>(out peg))
        {
            Destroy(peg.gameObject);
        }
    }
}
