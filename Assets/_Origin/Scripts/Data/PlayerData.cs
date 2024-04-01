using UnityEngine;
using UnityEngine.Events;

public class PlayerData: MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    public UnityEvent onPlayerUsernameChanged = new UnityEvent();
    public UnityEvent onPlayerBestScoreChanged = new UnityEvent();
    public UnityEvent onPlayerMoneyChanged = new UnityEvent();
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

    private string m_UserName;
    private int m_BestScore;
    private int m_Money;

    public string UserName
    {
        get { return m_UserName; }
        set
        {
            m_UserName = value;
            onPlayerUsernameChanged.Invoke();
        }
    }

    public int BestScore
    {
        get { return m_BestScore; }
        set
        {
            m_BestScore = value;
            onPlayerBestScoreChanged.Invoke();
        }
    }

    public int Money
    {
        get { return m_Money; }
        set
        {
            m_Money = value;
            onPlayerMoneyChanged.Invoke();
        }
    }

    public void SetPlayerData(string userName, int bestScore, int money)
    {
        this.m_UserName = userName;
        this.m_BestScore = bestScore;
        this.m_Money = money;
    }
}