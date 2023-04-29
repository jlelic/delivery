using System.Collections.Generic;
using UnityEngine;

public class PegManager : MonoBehaviour
{
    #region Singleton set-up

    private static PegManager instance;

    public static PegManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PegManager>();
            }
            return instance;
        }
    }

    #endregion

    [Header("Refs")]
    [SerializeField] private PegBullet bulletPrefab;
    [SerializeField] private PegKeyboard pegKeyboard;
    [SerializeField] private Transform bulletInitPos;

    [Header("Data")]
    [SerializeField] private IntegerVariable bulletCount;
    [SerializeField] private IntegerVariable maxBulletCount;
    [SerializeField] private IntegerVariable maxBulletModifier;

    private HashSet<LETTER> collectedLetters = new HashSet<LETTER>();

    private GameObject currentLayout;

    public void SetNewLevel(int maxBullets, GameObject layoutPrefab)
    {
        collectedLetters.Clear();
        pegKeyboard.ResetKeyboard();
        maxBulletCount.SetValue(maxBullets + maxBulletModifier.value);
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
        if (!collectedLetters.Contains(newLetter))
        {
            collectedLetters.Add(newLetter);
            pegKeyboard.ShowLetter(newLetter);
        }
    }
}
