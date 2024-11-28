using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float gameTime = 60f;
    public Text timerText;
    public Text gameOverText;
    public ScoreManager scoreManager;
    public BallManager ballManager;

    private float remainingTime;

    void Start()
    {
        remainingTime = gameTime;
        StartCoroutine(GameTimer());
    }

    void Update()
    {
        timerText.text = "Time: " + Mathf.RoundToInt(remainingTime);

        if (remainingTime <= 0f)
        {
            EndGame();
        }
    }

    IEnumerator GameTimer()
    {
        while (remainingTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }
    }

    void EndGame()
    {
        gameOverText.gameObject.SetActive(true);
        int highestScore = 0;
        int winningPlayerId = -1;
        for (int i = 0; i < scoreManager.playerScores.Length; i++)
        {
            if (scoreManager.playerScores[i] > highestScore)
            {
                highestScore = scoreManager.playerScores[i];
                winningPlayerId = i;
            }
        }

        if (winningPlayerId != -1)
        {
            gameOverText.text = "Player " + PhotonNetwork.PlayerList[winningPlayerId].NickName + " wins!";
        }
        else
        {
            gameOverText.text = "It's a tie!";
        }

        // Disable ball spawning and teleportation
        ballManager.enabled = false;
    }
}