using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton set-up

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    #endregion

    [SerializeField] PegBall pegPrefab;

    private List<LETTER> collectedLetters;

    private void Awake()
    {
        collectedLetters = new List<LETTER>();
        Instantiate<PegBall>(pegPrefab);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Instantiate<PegBall>(pegPrefab);
        }
    }

    public void AddLetter(LETTER newLetter)
    {
        collectedLetters.Add(newLetter);
        Debug.Log(collectedLetters.Count);
    }
}
