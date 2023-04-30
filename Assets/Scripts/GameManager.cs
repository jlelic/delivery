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
    [SerializeField] public PegManager pegGameManager;
    [SerializeField] public JokeHandler jokeHandler;

    [SerializeField] private List<Level> levels;

    private int currentIndex = 0;

    public void LoadNextLevel()
    {
        pegGameManager.gameObject.SetActive(false);

        Level currentLevel = levels[currentIndex];
        pegGameManager.SetNewLevel(currentLevel.data.maxBullets, currentLevel.pegLayout);
        jokeHandler.SetUpNewLevel(currentIndex);
        currentIndex++;
        pegGameManager.gameObject.SetActive(true);

        MusicMixer.instance.StartMusic();
    }

    public void ShowJokeCreator(HashSet<LETTER> collectedLetters)
    {

        jokeHandler.ShowPunchlineInput(collectedLetters);
        MusicMixer.instance.QueueHigh();
    }

    private void Start()
    {
        Utils.SetTimeout(this, 0.2f, () => LoadNextLevel());
    }
}

[Serializable]
public class Level
{
    [SerializeField] public LevelData data;
    [SerializeField] public GameObject pegLayout;
}

