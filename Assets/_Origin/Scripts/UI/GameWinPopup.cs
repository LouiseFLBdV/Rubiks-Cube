using RubicksCubeCreator;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameWinPopup : MonoBehaviour
{
    [SerializeField] private Text gameScore;
    [SerializeField] private Text coinsEarned;
    [SerializeField] private int win3Stars;
    [SerializeField] private int win2Stars;
    private Animator m_Animator;
    private Cube m_Cube;

    private void Start()
    {
        GameManager.Instance.OnGameFinished.AddListener(RenderScore);
        m_Animator = GetComponent<Animator>();
        m_Cube = FindObjectOfType<Cube>();
        Debug.Log("Start");
    }

    private void RenderScore(string username, int gameScore)
    {
        Debug.Log("RenderScore");
        m_Cube.gameObject.SetActive(false);
        gameObject.SetActive(true);
        var stars = 0;
        
        if (gameScore < win2Stars)
        {
            m_Animator.SetTrigger("1Star");
            stars = 1;
        }
        else if (gameScore >= win3Stars)
        {
            m_Animator.SetTrigger("3Star");
            stars = 3;
        }
        else
        {
            stars = 2;
            m_Animator.SetTrigger("2Star");
        }

        var coins = Random.Range(5, 15 * stars);
        this.gameScore.text = gameScore.ToString();
        this.coinsEarned.text = coins.ToString();
        PlayerData.Instance.Money += coins;
    }

    public void NextButtonClicked()
    {
        GameManager.Instance.ChangeGameState(GameManager.GameState.Game);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameFinished.RemoveListener(RenderScore);
    }
}