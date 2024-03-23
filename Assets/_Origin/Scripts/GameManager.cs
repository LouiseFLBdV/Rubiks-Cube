using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event Action<GameState> OnGameStateChanged;
    public static int currentScore;
     
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
        ChangeGameState(GameState.Menu, 0);
    }
    
    public void ChangeGameState(GameState state, int score)
    {
        currentScore = score;
        Cursor.lockState = CursorLockMode.None;
        switch (state)
        {
            case GameState.Menu:
                break;
            case GameState.Game:
                break;
            case GameState.Lose:
                break;
            case GameState.Win:
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
}
