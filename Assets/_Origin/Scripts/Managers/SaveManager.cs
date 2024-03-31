using UnityEngine;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public UnityEvent onDataSaved;
    public UnityEvent onNoSaveData;

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
        PlayerData.Instance.UserName = PlayerPrefs.GetString("userName", "player");
        PlayerData.Instance.BestScore = PlayerPrefs.GetInt("bestScore", 0);
        PlayerData.Instance.Money = PlayerPrefs.GetInt("userMoney", 0);
        if (PlayerData.Instance.UserName == "player" && PlayerData.Instance.BestScore == 0 && PlayerData.Instance.Money == 0)
        {
            Debug.Log(PlayerPrefs.GetString("userName", "player"));
            onNoSaveData?.Invoke();
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("userName", PlayerData.Instance.UserName);
        PlayerPrefs.SetInt("bestScore", PlayerData.Instance.BestScore);
        PlayerPrefs.SetInt("userMoney", PlayerData.Instance.Money);
        PlayerPrefs.Save();
        onDataSaved?.Invoke();
    }
}