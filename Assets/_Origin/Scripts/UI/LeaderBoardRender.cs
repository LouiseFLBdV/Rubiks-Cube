using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class LeaderBoardRender : MonoBehaviour
{
    public GameObject userRankTemplate;
    public GameObject playerRankTemplate;
    public Transform userRankParentRect;

    void Start()
    {
        LeaderBoardManager.Instance.OnPlayersDataLoaded.AddListener(RenderLeaderBoard);
        LeaderBoardManager.Instance.OnPlayerDataLoaded.AddListener(RenderPlayerRank);
    }

    public void RenderLeaderBoard(List<PlayerLeaderboardData> playersDataLeaderBoard)
    {
        foreach (PlayerLeaderboardData playerDataLeaderBoard in playersDataLeaderBoard)
        {
            var player = Instantiate(userRankTemplate);
            player.SetActive(true);
            player.transform.SetParent(userRankParentRect, false);
            player.GetComponent<PlayerRankUI>().SetPlayerLeaderboardData(playerDataLeaderBoard);
        }
    }
    public void RenderPlayerRank(PlayerLeaderboardData playerRank)
    {
        playerRankTemplate.GetComponent<PlayerRankUI>().SetPlayerLeaderboardData(playerRank);
    }
}
