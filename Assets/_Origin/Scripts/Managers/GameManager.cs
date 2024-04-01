using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UnityEvent<GameState> OnGameStateChanged = new UnityEvent<GameState>();
    public UnityEvent<string,int> OnGameFinished = new UnityEvent<string, int>();
    
    // PlayerLeaderBoard and current data state
    public List<PlayerLeaderboardWrapper> PlayersLeaderboardData;
    public int currentScore;
    public int playerCounter;
    public GameState CurrentState;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LeaderBoardManager.Instance.OnPlayersDataLoaded.AddListener(GetPlayersFromLeaderBoard);
        LeaderBoardManager.Instance.OnPlayerDataLoaded.AddListener(GetPlayersFromLeaderBoard);
        ChangeGameState(GameState.Menu);
    }

    private void GetPlayersFromLeaderBoard(List<PlayerLeaderboardWrapper> playersData)
    {
        this.PlayersLeaderboardData = playersData;
    }
    private void GetPlayersFromLeaderBoard(PlayerLeaderboardWrapper playerLeaderboardWrapper)
    {
        // bestScore = playerLeaderboardData.bestScore;
    }
    
    public void ChangeGameState(GameState state)
    {
        Cursor.lockState = CursorLockMode.None;
        switch (state)
        {
            case GameState.Menu:
                SceneManager.LoadScene(0);
                currentScore = 0;
                playerCounter = 0;
                break;
            case GameState.Game:
                SceneManager.LoadScene(1);
                currentScore = 0;
                playerCounter = 0;
                break;
            case GameState.Lose:
                currentScore = 0;
                playerCounter = 0;
                break;
            case GameState.Win:
                if (currentScore > PlayerData.Instance.BestScore)
                {
                    SaveScore();
                }
                currentScore = 0;
                playerCounter = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        
        OnGameStateChanged?.Invoke(state);
    }
    
    public enum GameState
    {
        Menu,
        Game,
        Lose,
        Win 
    }

    public void SaveScore()
    {
        OnGameFinished.Invoke(PlayerData.Instance.UserName, currentScore);
    }
}
