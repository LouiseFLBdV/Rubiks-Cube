using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UnityEvent<GameState> OnGameStateChanged = new UnityEvent<GameState>();
    public UnityEvent<string,int> OnGameFinished = new UnityEvent<string, int>();
    public GameState CurrentState;

    public List<PlayerLeaderboardData> playersData;
    public int currentScore;
    public int bestScore;
    public string userName;
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
        this.playersData = playersData;
    }
    private void GetPlayersFromLeaderBoard(PlayerLeaderboardData playerLeaderboardData)
    {
        bestScore = playerLeaderboardData.bestScore;
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
                if (currentScore > bestScore)
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
        Debug.LogError("CubedInvoked");
        OnGameFinished.Invoke(userName, currentScore);
    }
}
