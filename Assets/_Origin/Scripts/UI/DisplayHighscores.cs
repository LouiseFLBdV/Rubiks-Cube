using System.Collections;
using UnityEngine;

public class DisplayHighscores : MonoBehaviour 
{
    public TMPro.TextMeshProUGUI[] rNames;
    public TMPro.TextMeshProUGUI[] rScores;
    LeaderBoardManager myScores;

    void Start()
    {
        for (int i = 0; i < rNames.Length;i ++)
        {
            rNames[i].text = i + 1 + ". Fetching...";
        }
        myScores = GetComponent<LeaderBoardManager>();
        StartCoroutine("RefreshHighscores");
    }
    public void SetScoresToMenu(PlayerData[] highscoreList)
    {
        for (int i = 0; i < rNames.Length;i ++)
        {
            rNames[i].text = i + 1 + ". ";
            if (highscoreList.Length > i)
            {
                rScores[i].text = highscoreList[i].bestScore.ToString();
                rNames[i].text = highscoreList[i].userName;
            }
        }
    }
    IEnumerator RefreshHighscores() //Refreshes the scores every 30 seconds
    {
        while(true)
        {
            // myScores.DownloadScores();
            yield return new WaitForSeconds(30);
        }
    }
}
