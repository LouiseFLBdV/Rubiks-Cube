using UnityEngine;
using UnityEngine.UI;

public class PlayerRankUI : MonoBehaviour
{
    [SerializeField] private Text userName;
    [SerializeField] private Text rankedPos;
    [SerializeField] private Text bestScore;

    public void SetPlayerLeaderboardData(PlayerLeaderboardWrapper playerLeaderboardWrapper)
    {
        userName.text = playerLeaderboardWrapper.userName;
        rankedPos.text = playerLeaderboardWrapper.rankedPosition.ToString();
        bestScore.text = playerLeaderboardWrapper.bestScore.ToString();
    }
}