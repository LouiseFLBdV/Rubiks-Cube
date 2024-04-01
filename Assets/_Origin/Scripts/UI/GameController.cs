using RubicksCubeCreator;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text playerCounter;
    public Text playerTips;
    public Button tipsButton;
    private Cube m_Cube;
    private void Awake()
    {
        m_Cube = FindObjectOfType<Cube>();
    }

    private void Start()
    {
        SaveManager.Instance.onLoadedSaveData.AddListener(RenderPlayerTips);
        PlayerData.Instance.onPlayerTipsChanged.AddListener(RenderPlayerTips);
        m_Cube.onShuffleEnd.AddListener(GameStart);
        m_Cube.OnCubeSolved.AddListener(CubeSolved);
        GameManager.Instance.OnGameFinished.AddListener(RenderWinPopup);
    }

    private void Initialize()
    {
        if (SaveManager.Instance.isDataLoaded)
        {
            RenderPlayerTips();
        }
        RenderPlayerCounter();
        m_Cube.Shuffle(true);
    }

    private void CubeSolved()
    {
        GameManager.Instance.ChangeGameState(GameManager.GameState.Win);
    }
    
    public void CubeChanged()
    {
        GameManager.Instance.playerCounter++;
        RenderPlayerCounter();
    }

    public void GameStart()
    {
        m_Cube.OnCubeChanged.AddListener(CubeChanged);
    }
    
    public void RenderPlayerCounter()
    {
        playerCounter.text = GameManager.Instance.playerCounter.ToString();
    }

    public void RenderPlayerTips()
    {
        Debug.LogError(1);
        playerTips.text = PlayerData.Instance.Tips.ToString();
        if (PlayerData.Instance.Tips <= 0)
        {
            tipsButton.interactable = false;
        }
        else
        {
            tipsButton.interactable = true;
        }
    }

    public void RenderWinPopup(string username, int playerScore)
    {
        
    }
    
    public void MenuButtonClicked()
    {
        GameManager.Instance.ChangeGameState(GameManager.GameState.Menu);
    }

    public void AddTips()
    {
        PlayerData.Instance.Tips += 1;
    }

    public void GetTips()
    {
        if (PlayerData.Instance.Tips >= 0)
        {
            PlayerData.Instance.Tips -= 1;
            m_Cube.SolveOneStep();
        }
    }
    
    private void OnDestroy()
    {
        SaveManager.Instance.onLoadedSaveData.RemoveListener(RenderPlayerTips);
        PlayerData.Instance.onPlayerTipsChanged.RemoveListener(RenderPlayerTips);
        m_Cube.OnCubeChanged.RemoveListener(CubeChanged);
        m_Cube.onShuffleEnd.RemoveListener(GameStart);
        m_Cube.OnCubeSolved.RemoveListener(CubeSolved);
        GameManager.Instance.OnGameFinished.RemoveListener(RenderWinPopup);
    }
}