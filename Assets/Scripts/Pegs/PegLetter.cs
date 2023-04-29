using UnityEngine;

public class PegLetter : MonoBehaviour
{
    [SerializeField] private LETTER letter;

    private void OnCollisionEnter2D(Collision2D other)
    {
        PegGameManager.Instance.AddLetter(letter);
        Destroy(gameObject);
    }
}