using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] public MenuManager menuManager;
    [SerializeField] public AudienceManager audienceManager;

    [SerializeField] private List<GameObject> levels;

    [Header("Important")]
    public GameObject tomato;
    public Boolean inputEnabled = true;
    private int currentIndex = 0;
    public int totalScore;
    Guid gameId;

    [SerializeField] private UnityEvent gameStartEvent;

    public void StartNewGame()
    {
        gameId = System.Guid.NewGuid();
        audienceManager.RandomizeAudience();
        currentIndex = 0;
        gameStartEvent.Invoke();
        Utils.SetTimeout(this, 3f, () =>
        {
            LoadNextLevel();
        });
    }


    public void LoadNextLevel()
    {
        if (currentIndex == levels.Count)
        {
            menuManager.ShowOutro(totalScore);
            return;
        }
        pegGameManager.gameObject.SetActive(false);

        pegGameManager.SetNewLevel(levels[currentIndex]);
        jokeHandler.SetUpNewLevel(currentIndex, gameId);
        currentIndex++;
        pegGameManager.gameObject.SetActive(true);
    }

    public void ShowJokeCreator(HashSet<LETTER> collectedLetters)
    {

        jokeHandler.ShowPunchlineInput(collectedLetters);
    }

    private void Start()
    {
        menuManager.gameObject.SetActive(true);
        //Utils.SetTimeout(this, 0.2f, () => LoadNextLevel());
    }

    public void DisableInput()
    {
        inputEnabled = false;
    }

    public void EnableInput()
    {
        inputEnabled = true;
    }
}