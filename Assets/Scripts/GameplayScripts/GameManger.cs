using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public enum GameState { Starting, Playing, GameOver }
    public GameState CurrentState { get; private set; }
    
    public GameObject scoreSummaryScreen;

    private float gameTime;
    private float startTime = 60f; // 1 minute

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
            CurrentState = GameState.Starting;
            gameTime = startTime;
        }
    }

    private void Update()
    {
        if (CurrentState == GameState.Playing)
        {
            gameTime -= Time.deltaTime;

            if (gameTime <= 0f)
            {
                CurrentState = GameState.GameOver;
            }
        }
    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
    }

    public bool IsGameOver()
    {
        return CurrentState == GameState.GameOver;
    }

    public float GetRemainingTime()
    {
        return gameTime;
    }
}