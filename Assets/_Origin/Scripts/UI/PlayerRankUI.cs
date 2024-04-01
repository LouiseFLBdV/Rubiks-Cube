using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerRankUI : MonoBehaviour
{
    [SerializeField] private Text userName;
    [SerializeField] private Text rankedPos;
    [SerializeField] private Text bestScore;

    public void SetPlayerLeaderboardData(PlayerLeaderboardData playerLeaderboardData)
    {
        userName.text = playerLeaderboardData.userName;
        rankedPos.text = playerLeaderboardData.rankedPosition.ToString();
        bestScore.text = playerLeaderboardData.bestScore.ToString();
    }
}