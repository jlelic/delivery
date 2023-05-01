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
    [SerializeField] private Transform pegBoard;
    [SerializeField] private Transform bulletInitPos;
    [SerializeField] public ParticleSystem particleTemplate;

    [Header("Data")]
    [SerializeField] private IntegerVariable bulletCount;
    [SerializeField] private IntegerVariable maxBulletCount;
    [SerializeField] private FloatVariable gravityScale;
    [SerializeField] private FloatVariable bulletScale;

    private HashSet<LETTER> collectedLetters = new HashSet<LETTER>();

    private GameObject currentLayout;

    public void OnGameStart()
    {
        maxBulletCount.ResetValue();
        gravityScale.ResetValue();
        bulletScale.ResetValue();
    }

    public void SetNewLevel(GameObject layoutPrefab)
    {
        collectedLetters.Clear();
        pegKeyboard.ResetKeyboard();
        bulletCount.SetValue(maxBulletCount.value);

        currentLayout = Instantiate(layoutPrefab, pegBoard);
        currentLayout.transform.localPosition = new Vector3(0f, -1f, -0.5f);
    }

    public void Enable()
    {
        Instantiate<PegBullet>(bulletPrefab, bulletInitPos.position, Quaternion.identity);
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
            Utils.SetTimeout(this, 2, () =>
            {

                Destroy(currentLayout.gameObject);
                currentLayout = null;
                // Start Joke creation gameplay
                GameManager.Instance.ShowJokeCreator(collectedLetters);
            });
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
