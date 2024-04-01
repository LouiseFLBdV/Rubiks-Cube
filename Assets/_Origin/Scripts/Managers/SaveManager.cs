using UnityEngine;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public UnityEvent onDataSaved = new UnityEvent();
    public UnityEvent onNoSaveData = new UnityEvent();
    public UnityEvent onLoadedSaveData = new UnityEvent();

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
        PlayerData.Instance.onPlayerMoneyChanged.AddListener(SaveData);
        PlayerData.Instance.onPlayerUsernameChanged.AddListener(SaveData);
        PlayerData.Instance.onPlayerBestScoreChanged.AddListener(SaveData);
        Initialize();
    }

    private void Initialize()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            PlayerData.Instance.SetPlayerData(PlayerPrefs.GetString("username", "player"),
                PlayerPrefs.GetInt("bestScore", 0), PlayerPrefs.GetInt("userMoney", 0));
        }
        else
        {
            PlayerPrefs.DeleteAll();
            PlayerData.Instance.SetPlayerData(PlayerPrefs.GetString("username", "player"),
                PlayerPrefs.GetInt("bestScore", 0), PlayerPrefs.GetInt("userMoney", 0));
            onNoSaveData?.Invoke();
        }
        onLoadedSaveData?.Invoke();
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("username", PlayerData.Instance.UserName);
        PlayerPrefs.SetInt("bestScore", PlayerData.Instance.BestScore);
        PlayerPrefs.SetInt("userMoney", PlayerData.Instance.Money);
        PlayerPrefs.Save();
        onDataSaved?.Invoke();
    }
}