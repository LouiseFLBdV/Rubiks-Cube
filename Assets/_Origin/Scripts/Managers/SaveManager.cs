using System;
using UnityEngine;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public UnityEvent onDataSaved = new UnityEvent();
    public UnityEvent onNoSaveData = new UnityEvent();
    public UnityEvent onLoadedSaveData = new UnityEvent();
    public bool isDataLoaded = false;
    
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
        PlayerData.Instance.onPlayerTipsChanged.AddListener(SaveData);
        LoadPlayerData();
    }

    public void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            PlayerData.Instance.SetPlayerData(PlayerPrefs.GetString("username", "player"),
                PlayerPrefs.GetInt("bestScore", 0), PlayerPrefs.GetInt("money", 0), PlayerPrefs.GetInt("tips", 0));
        }
        else
        {
            PlayerPrefs.DeleteAll();
            PlayerData.Instance.SetPlayerData(PlayerPrefs.GetString("username", "player"),
                PlayerPrefs.GetInt("bestScore", 0), PlayerPrefs.GetInt("money", 0), PlayerPrefs.GetInt("tips", 0));
            onNoSaveData?.Invoke();
        }

        isDataLoaded = true;
        onLoadedSaveData?.Invoke();
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("username", PlayerData.Instance.UserName);
        PlayerPrefs.SetInt("bestScore", PlayerData.Instance.BestScore);
        PlayerPrefs.SetInt("money", PlayerData.Instance.Money);
        PlayerPrefs.SetInt("tips", PlayerData.Instance.Tips);
        PlayerPrefs.Save();
        onDataSaved?.Invoke();
    }

    private void OnDestroy()
    {
        PlayerData.Instance.onPlayerMoneyChanged.RemoveListener(SaveData);
        PlayerData.Instance.onPlayerUsernameChanged.RemoveListener(SaveData);
        PlayerData.Instance.onPlayerBestScoreChanged.RemoveListener(SaveData);
        PlayerData.Instance.onPlayerTipsChanged.RemoveListener(SaveData);
    }
}