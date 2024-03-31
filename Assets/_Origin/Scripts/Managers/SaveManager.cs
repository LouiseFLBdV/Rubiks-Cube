using UnityEngine;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public UnityEvent onDataSaved;
    public UnityEvent onNoSaveData;

    private PlayerData m_PlayerData;

    public PlayerData PlayerData
    {
        get
        {
            if (m_PlayerData == null)
                m_PlayerData = new PlayerData();
            return m_PlayerData;
        }
    }

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

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerData.UserName = PlayerPrefs.GetString("userName", "player");
        PlayerData.BestScore = PlayerPrefs.GetInt("bestScore", 0);
        PlayerData.Money = PlayerPrefs.GetInt("userMoney", 0);
        if (PlayerData.UserName == "player" && PlayerData.BestScore == 0 && PlayerData.Money == 0)
        {
            Debug.Log(PlayerPrefs.GetString("userName", "player"));
            onNoSaveData?.Invoke();
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("userName", PlayerData.UserName);
        PlayerPrefs.SetInt("bestScore", PlayerData.BestScore);
        PlayerPrefs.SetInt("userMoney", PlayerData.Money);
        PlayerPrefs.Save();
        onDataSaved?.Invoke();
    }
}