using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UnityEvent<GameState> OnGameStateChanged = new UnityEvent<GameState>();
    public UnityEvent<string,int> OnGameFinished = new UnityEvent<string, int>();
    
    // PlayerLeaderBoard and current data state
    public List<PlayerLeaderboardData> PlayersLeaderboardData;
    public int currentScore;
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
        CurrentState = GameState.Menu;
        // ChangeGameState(GameState.Menu);
        LeaderBoardManager.Instance.OnPlayersDataLoaded.AddListener(GetPlayersFromLeaderBoard);
        LeaderBoardManager.Instance.OnPlayerDataLoaded.AddListener(GetPlayersFromLeaderBoard);
    }

    private void GetPlayersFromLeaderBoard(List<PlayerLeaderboardData> playersData)
    {
        this.PlayersLeaderboardData = playersData;
    }
    private void GetPlayersFromLeaderBoard(PlayerLeaderboardData playerLeaderboardData)
    {
        // bestScore = playerLeaderboardData.bestScore;
    }
    
    public void ChangeGameState(GameState state)
    {
        Cursor.lockState = CursorLockMode.None;
        switch (state)
        {
            case GameState.Menu:
                currentScore = 0;
                break;
            case GameState.Game:
                currentScore = 0;
                break;
            case GameState.Lose:
                currentScore = 0;
                break;
            case GameState.Win:
                if (currentScore > PlayerData.Instance.BestScore)
                {
                    SaveScore();
                }
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
