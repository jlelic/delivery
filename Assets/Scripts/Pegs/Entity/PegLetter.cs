using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PegLetter : MonoBehaviour
{
    [SerializeField] private LETTER letter;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PegManager.Instance.AddLetter(letter);
            Destroy(gameObject);
        }
    }
}
