public class PlayerLeaderboardWrapper
{
    public string userName;
    public int bestScore;
    public int rankedPosition;
    
    public PlayerLeaderboardWrapper(string userName, int bestScore, int rankedPosition)
    {
        this.bestScore = bestScore;
        this.userName = userName;
        this.rankedPosition = rankedPosition;
    }
}