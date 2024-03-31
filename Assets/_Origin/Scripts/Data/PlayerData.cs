public class PlayerData
{
    public string userName;
    public int bestScore;
    
    public PlayerData(string userName, int bestScore)
    {
        this.bestScore = bestScore;
        this.userName = userName;
    }

    public override string ToString()
    {
        return $"{nameof(userName)}: {userName}, {nameof(bestScore)}: {bestScore}";
    }
}