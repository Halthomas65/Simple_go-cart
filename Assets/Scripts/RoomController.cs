using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
public class RoomController : MonoBehaviourPunCallbacks
{
    //Player instance prefab, must be located in the Resources folder
    public GameObject[] playerPrefabs;
    //Player spawn point
    public Transform[] spawnPoints;
    string lobbyScene = "GameLobby";

    void Start()
    {
        //In case we started this demo with the wrong scene being active, simply load the menu scene
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("Not in the room, returning back to Lobby");
            UnityEngine.SceneManagement.SceneManager.LoadScene(lobbyScene);
            return;
        }
        //We're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        StartCoroutine(LateSpawn());
    }

    IEnumerator LateSpawn()
    {
        yield return new WaitForSeconds(2);

        //We're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
        PhotonNetwork.Instantiate(playerPrefabs[Random.Range(0, playerPrefabs.Length - 1)].name, spawnPoint.position, spawnPoint.rotation, 0);

    }

    void OnGUI()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        //Leave this Room
        if (GUI.Button(new Rect(5, 5, 125, 25), "Leave Room"))
        {
            PhotonNetwork.LeaveRoom();
        }

        //Show the Room name
        GUI.Label(new Rect(135, 5, 200, 25), PhotonNetwork.CurrentRoom.Name);

        //Show the list of the players connected to this Room
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //Show if this player is a Master Client. There can only be one Master Client per Room so use this to define the authoritative logic etc.)
            string isMasterClient = (PhotonNetwork.PlayerList[i].IsMasterClient ? ": MasterClient" : "");
            GUI.Label(new Rect(5, 35 + 30 * i, 200, 25), PhotonNetwork.PlayerList[i].NickName + isMasterClient);
        }

        // Show Current Game state of the room
        // Show the remaining time from the GameManager at the upper center of the screen
        GUI.Label(new Rect(Screen.width / 2, 5, 200, 25), "REMAINING TIME: " + GameManager.Instance.GetRemainingTime() + "s");
        
        // Show current score of the current player at the bottom left of the screen
        GUI.Label(new Rect(5, Screen.height - 30, 200, 25), "Score: " + ScoreManager.Instance.GetScore(PhotonNetwork.LocalPlayer.ActorNumber));        
    }

    public override void OnLeftRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(lobbyScene);
    }
}