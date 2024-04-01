using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.Events;

public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager Instance;

    private string leaderboardId = "Rubiks_Leaderboard";
    public UnityEvent<PlayerLeaderboardWrapper> OnPlayerDataLoaded;
    public UnityEvent<List<PlayerLeaderboardWrapper>> OnPlayersDataLoaded;

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
        AuthenticationManager.Instance.OnAuthenticated.AddListener(GetPlayerScoreFromLeaderBoard);
        AuthenticationManager.Instance.OnAuthenticated.AddListener(GetAllScoresFromLeaderBoard);
        GameManager.Instance.OnGameFinished.AddListener(AddScoreToLeaderBoard);
    }
    
    public async void AddScoreToLeaderBoard(string username, int score)
    {
        // TODO change metadata when authentication will be ready
        var metadata = new Dictionary<string, string>();
        metadata.Add("username", username);
        var playerEntry = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(
                leaderboardId,
                score,
                new AddPlayerScoreOptions { Metadata = metadata });
    }

    public async void GetPlayerScoreFromLeaderBoard()
    {
        var scoreResponse = await LeaderboardsService.Instance
            .GetPlayerScoreAsync(
                leaderboardId,
                new GetPlayerScoreOptions { IncludeMetadata = true });

        PlayerLeaderboardWrapper currentPlayerLeaderboard = new PlayerLeaderboardWrapper(
            JsonConvert.DeserializeObject<Dictionary<string, string>>(scoreResponse.Metadata)["username"],
            (int)scoreResponse.Score,
            scoreResponse.Rank);
        OnPlayerDataLoaded.Invoke(currentPlayerLeaderboard);
    }

    public async void GetAllScoresFromLeaderBoard()
    {
        var scoreResponse = await LeaderboardsService.Instance
            .GetScoresAsync(
                leaderboardId,
                new GetScoresOptions { IncludeMetadata = true });
        List<PlayerLeaderboardWrapper> playersData = new List<PlayerLeaderboardWrapper>();
        foreach (var entry in scoreResponse.Results)
        {
            PlayerLeaderboardWrapper playerLeaderboardWrapper = new PlayerLeaderboardWrapper(
                JsonConvert.DeserializeObject<Dictionary<string, string>>(entry.Metadata)["username"],
                (int)entry.Score,
                entry.Rank);
            playerLeaderboardWrapper.rankedPosition = entry.Rank;
            playersData.Add(playerLeaderboardWrapper);
        }

        OnPlayersDataLoaded.Invoke(playersData);
    }

    private void OnDestroy()
    {
        AuthenticationManager.Instance.OnAuthenticated.RemoveListener(GetPlayerScoreFromLeaderBoard);
        AuthenticationManager.Instance.OnAuthenticated.RemoveListener(GetAllScoresFromLeaderBoard);
        GameManager.Instance.OnGameFinished.RemoveListener(AddScoreToLeaderBoard);

    }
}