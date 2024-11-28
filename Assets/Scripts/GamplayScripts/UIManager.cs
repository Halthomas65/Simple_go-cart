using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text[] playerScoreTexts;
    public ScoreManager scoreManager;

    void Update()
    {
        for (int i = 0; i < scoreManager.playerScores.Length; i++)
        {
            playerScoreTexts[i].text = "Player " + PhotonNetwork.PlayerList[i].NickName + ": " + scoreManager.playerScores[i];
        }
    }
}