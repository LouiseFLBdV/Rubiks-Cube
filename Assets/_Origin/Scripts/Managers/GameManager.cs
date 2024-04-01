using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UnityEvent<GameState> OnGameStateChanged = new UnityEvent<GameState>();
    public UnityEvent<string, int> OnGameFinished = new UnityEvent<string, int>();

    // PlayerLeaderBoard and current data state
    public List<PlayerLeaderboardWrapper> PlayersLeaderboardData;
    public int currentScore;
    public int playerCounter;
    public int tipsUsed;
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
                InitializeCurrentData();
                break;
            case GameState.Game:
                SceneManager.LoadScene(1);
                InitializeCurrentData();
                break;
            case GameState.Lose:
                InitializeCurrentData();
                break;
            case GameState.Win:
                WinController();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        SaveManager.Instance.LoadPlayerData();
        OnGameStateChanged?.Invoke(state);
    }

    private void InitializeCurrentData()
    {
        currentScore = 0;
        playerCounter = 0;
        tipsUsed = 0;
    }

    private void WinController()
    {
        // Todo implement a better score calculation logic
        currentScore = 123456 / (tipsUsed + 1) / playerCounter;
        OnGameFinished.Invoke(PlayerData.Instance.UserName, currentScore);
        Debug.Log(currentScore);
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

    private void OnDestroy()
    {
        LeaderBoardManager.Instance.OnPlayersDataLoaded.RemoveListener(GetPlayersFromLeaderBoard);
        LeaderBoardManager.Instance.OnPlayerDataLoaded.RemoveListener(GetPlayersFromLeaderBoard);
    }
}