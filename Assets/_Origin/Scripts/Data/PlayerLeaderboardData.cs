public class PlayerLeaderboardData
{
    public string userName;
    public int bestScore;
    public int rankedPosition;
    
    public PlayerLeaderboardData(string userName, int bestScore)
    {
        this.bestScore = bestScore;
        this.userName = userName;
    }

    public override string ToString()
    {
        return $"{nameof(userName)}: {userName}, {nameof(bestScore)}: {bestScore}";
    }
}