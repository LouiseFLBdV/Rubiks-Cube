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
            onPlayerUsernameChanged.Invoke();
            m_UserName = value;
        }
    }

    public int BestScore
    {
        get { return m_BestScore; }
        set
        {
            onPlayerBestScoreChanged.Invoke();
            m_BestScore = value;
        }
    }

    public int Money
    {
        get { return m_Money; }
        set
        {
            onPlayerMoneyChanged.Invoke();
            m_Money = value;
        }
    }
}