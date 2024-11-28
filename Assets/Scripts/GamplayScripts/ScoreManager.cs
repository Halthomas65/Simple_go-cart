using Photon.Pun;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int[] playerScores;

    void Start()
    {
        playerScores = new int[PhotonNetwork.PlayerList.Length];
    }

    public void AddPoint(int playerId)
    {
        playerScores[playerId]++;
    }
}