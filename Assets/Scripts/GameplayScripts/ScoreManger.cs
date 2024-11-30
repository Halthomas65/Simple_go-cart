using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }

    private Dictionary<int, int> playerScores;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            playerScores = new Dictionary<int, int>();
        }
    }
    public void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;
    }

    public void AddPlayer(int playerId)
    {
        if (!playerScores.ContainsKey(playerId))
        {
            playerScores.Add(playerId, 0);
        }
    }

    public void UpdateScore(int playerId, int points)
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;
            
        if (playerScores.ContainsKey(playerId))
        {
            playerScores[playerId] += points;
        }
    }

    public int GetScore(int playerId)
    {
        if (playerScores.ContainsKey(playerId))
        {
            return playerScores[playerId];
        }
        return 0;
    }
}