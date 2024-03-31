public class PlayerData
{
    private string m_UserName;
    private int m_BestScore;
    private int m_Money;

    public string UserName
    {
        get { return m_UserName; }
        set { m_UserName = value; }
    }

    public int BestScore
    {
        get { return m_BestScore; }
        set { m_BestScore = value; }
    }

    public int Money
    {
        get { return m_Money; }
        set { m_Money = value; }
    }
}