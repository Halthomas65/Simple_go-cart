using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }

    [SerializeField] private Dictionary<int, int> playerScores;
    [SerializeField] private Dictionary<int, bool> playerStates; // 0 = dead, 1 = alive

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
            playerStates = new Dictionary<int, bool>();
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
            playerStates.Add(playerId, true); // Assume player is alive initially
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

    public void UpdateState(int playerId, bool state)    // Record if the player is dead or alive
    {
        if (playerStates.ContainsKey(playerId))
        {
            playerStates[playerId] = state;
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
    
    public bool GetState(int playerId)
    {
        if (playerStates.ContainsKey(playerId))
        {
            return playerStates[playerId];
        }
        return false;
    }
}