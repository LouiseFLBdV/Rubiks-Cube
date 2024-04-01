using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.Events;

public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager Instance;
    
    private string leaderboardId = "Rubiks_Leaderboard";
    public UnityEvent<PlayerLeaderboardData> OnPlayerDataLoaded;
    public UnityEvent<List<PlayerLeaderboardData>> OnPlayersDataLoaded;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        GameManager.Instance.OnGameFinished.AddListener((username, score) =>
        {
            if (AuthenticationManager.Instance.IsAuthenticated)
            {
                AddScoreToLeaderBoard(username, score);
            }
            else
            {
                Debug.LogError("User is not authenticated. Cannot upload score.");
            }
        });
        AuthenticationManager.Instance.OnAuthenticated.AddListener(GetPlayerScoreFromLeaderBoard);
        AuthenticationManager.Instance.OnAuthenticated.AddListener(GetAllScoresFromLeaderBoard);
    }

    public async void AddScoreToLeaderBoard(string username, int score)
    {
        PlayerLeaderboardData playerLeaderboardData = new PlayerLeaderboardData(username, score);
        
        var playerEntry = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(
                leaderboardId,
                score,
                new AddPlayerScoreOptions { Metadata = playerLeaderboardData});
    }

    public async void GetPlayerScoreFromLeaderBoard()
    {
        var scoreResponse = await LeaderboardsService.Instance
            .GetPlayerScoreAsync(
                leaderboardId,
                new GetPlayerScoreOptions { IncludeMetadata = true });
        
        PlayerLeaderboardData currentPlayerLeaderboard = JsonConvert.DeserializeObject<PlayerLeaderboardData>(scoreResponse.Metadata);
        currentPlayerLeaderboard.rankedPosition = scoreResponse.Rank;
        OnPlayerDataLoaded.Invoke(JsonConvert.DeserializeObject<PlayerLeaderboardData>(scoreResponse.Metadata));
    }
    
    public async void GetAllScoresFromLeaderBoard()
    {
        var scoreResponse = await LeaderboardsService.Instance
            .GetScoresAsync(
                leaderboardId,
                new GetScoresOptions { IncludeMetadata = true });
        List<PlayerLeaderboardData> playersData = new List<PlayerLeaderboardData>();
        foreach (var entry in scoreResponse.Results)
        {
            PlayerLeaderboardData playerLeaderboardData = JsonConvert.DeserializeObject<PlayerLeaderboardData>(entry.Metadata);
            playerLeaderboardData.rankedPosition = entry.Rank;
            playersData.Add(playerLeaderboardData);
        }
        OnPlayersDataLoaded.Invoke(playersData);
    }
}