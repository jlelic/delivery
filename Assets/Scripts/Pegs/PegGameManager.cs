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
    [SerializeField] private PegBullet bulletPrefab;
    [SerializeField] private Transform bulletInitPos;

    [Header("Data")]
    [SerializeField] private IntegerVariable bulletCount;
    [SerializeField] private IntegerVariable maxBulletCount;

    private HashSet<LETTER> collectedLetters = new HashSet<LETTER>();

    private GameObject currentLayout;

    public void SetNewLevel(int maxBullets, GameObject layoutPrefab)
    {
        collectedLetters.Clear();
        maxBulletCount.SetValue(maxBullets);
        bulletCount.SetValue(maxBullets);
        Instantiate<PegBullet>(bulletPrefab, bulletInitPos.position, Quaternion.identity);
        currentLayout = Instantiate(layoutPrefab, transform);
    }

    public void OnPegBulletDestroyed()
    {
        if (bulletCount.value > 0)
        {
            Instantiate<PegBullet>(bulletPrefab, bulletInitPos.position, Quaternion.identity);
        }
        else
        {
            // End Peg gameplay
            Destroy(currentLayout.gameObject);
            currentLayout = null;
            // Start Joke creation gameplay
            GameManager.Instance.ShowJokeCreator(collectedLetters);
        }
    }

    public void AddLetter(LETTER newLetter)
    {
        collectedLetters.Add(newLetter);
    }
}
