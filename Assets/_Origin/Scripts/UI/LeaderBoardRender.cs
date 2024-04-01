using System.Collections.Generic;
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

    public void RenderLeaderBoard(List<PlayerLeaderboardWrapper> playersDataLeaderBoard)
    {
        foreach (PlayerLeaderboardWrapper playerDataLeaderBoard in playersDataLeaderBoard)
        {
            var player = Instantiate(userRankTemplate);
            player.SetActive(true);
            player.transform.SetParent(userRankParentRect, false);
            player.GetComponent<PlayerRankUI>().SetPlayerLeaderboardData(playerDataLeaderBoard);
        }
    }
    public void RenderPlayerRank(PlayerLeaderboardWrapper playerRank)
    {
        playerRankTemplate.GetComponent<PlayerRankUI>().SetPlayerLeaderboardData(playerRank);
    }

    private void OnDestroy()
    {
        LeaderBoardManager.Instance.OnPlayersDataLoaded.RemoveListener(RenderLeaderBoard);
        LeaderBoardManager.Instance.OnPlayerDataLoaded.RemoveListener(RenderPlayerRank);
    }
}
