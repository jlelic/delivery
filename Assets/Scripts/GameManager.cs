using System;
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

    [Header("Managers")]
    [SerializeField] private PegGameManager pegGameManager;
    [SerializeField] private JokeHandler jokeHandler;

    [SerializeField] private List<Level> levels;

    private int currentIndex = 0;

    public void LoadNextLevel()
    {
        pegGameManager.gameObject.SetActive(false);

        Level currentLevel = levels[currentIndex];
        pegGameManager.SetNewLevel(currentLevel.data.maxBullets, currentLevel.pegLayout);
        jokeHandler.SetUpNewLevel(1, currentLevel);
        currentIndex++;
        pegGameManager.gameObject.SetActive(true);
    }

    public void ShowJokeCreator(HashSet<LETTER> collectedLetters)
    {
        Debug.Log("Collected letters:");
        foreach (LETTER letter in collectedLetters)
        {
            Debug.Log(letter.ToString());
        }
        jokeHandler.ShowPunchlineInput(collectedLetters);
        pegGameManager.gameObject.SetActive(false);
    }

    private void Awake()
    {
        LoadNextLevel();
    }

    private void Update()
    {
        // Simulate next level
        if (Input.GetKeyDown(KeyCode.N) && currentIndex < levels.Count)
        {
            LoadNextLevel();
        }
    }
}

[Serializable]
public class Level
{
    [SerializeField] public LevelData data;
    [SerializeField] public GameObject pegLayout;
}

