using System.Collections.Generic;
using UnityEngine;

public class PegGameManager : MonoBehaviour
{
    #region Singleton set-up

    private static PegGameManager instance;

    public static PegGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PegGameManager>();
            }
            return instance;
        }
    }

    #endregion

    [Header("Prefabs")]
    [SerializeField] private PegBullet pegPrefab;

    [Header("Data")]
    [SerializeField] private IntegerVariable bulletCount;

    private HashSet<LETTER> collectedLetters;

    private void Awake()
    {
        collectedLetters = new HashSet<LETTER>();
        Instantiate<PegBullet>(pegPrefab);
    }

    public void OnPegBulletDestroyed()
    {
        if (bulletCount.value > 0)
        {
            Instantiate<PegBullet>(pegPrefab);
        }
        else
        {
            // End Peg gameplay
            // Start Joke creation gameplay
            Debug.Log("Collected letters:");
            foreach (LETTER letter in collectedLetters)
            {
                Debug.Log(letter.ToString());
            }
        }
    }

    public void AddLetter(LETTER newLetter)
    {
        if (!collectedLetters.Contains(newLetter))
        {
            collectedLetters.Add(newLetter);
        }
    }
}
