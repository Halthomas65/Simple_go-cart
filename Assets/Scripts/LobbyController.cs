using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class LobbyController : MonoBehaviourPunCallbacks
{
    string playerName = "1";
    string gameVersion = "1";
    string sceneName = "Room";
    int MAX_PLAYER = 100;
    Vector2 roomListScroll = Vector2.zero;
    int Max_PLAYER_ROOM = 10;   // Maximum number of players in a room
    List<RoomInfo> createdRooms = new List<RoomInfo>();
    string roomName = "R1";
    bool isJoining = false;
    void Start()
    {
        playerName = "Player" + Random.Range(1, MAX_PLAYER);
        // Tinh Nang Dong bo
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu"; // Europe
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    private void OnGUI()
    {
        Rect lobbyPannel = new Rect(Screen.width / 2 - 450, Screen.height / 2 - 200, 900, 400);
        GUI.Window(0, lobbyPannel, LobbyWindow, "Lobby");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Mat ket noi" + cause);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Da ket noi");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    void LobbyWindow(int index)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Trạng thái " + PhotonNetwork.NetworkClientState);
        if (isJoining || !PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            GUI.enabled = false;
        }
        GUILayout.FlexibleSpace();
        roomName = GUILayout.TextField(roomName, GUILayout.Width(250));
        if (GUILayout.Button("Tạo phòng", GUILayout.Width(125)))
        {
            if (roomName != "")
            {
                isJoining = true;
                RoomOptions option = new RoomOptions();
                option.IsOpen = true;
                option.IsVisible = true;
                option.MaxPlayers = MAX_PLAYER / Max_PLAYER_ROOM;
                PhotonNetwork.JoinOrCreateRoom(roomName, option, TypedLobby.Default);
            }
        }
        GUILayout.EndHorizontal();
        roomListScroll = GUILayout.BeginScrollView(roomListScroll, true, true);
        // Room List
        if (createdRooms.Count == 0)
        {
            GUILayout.Label("Chưa có phòng nào");
        }
        else
        {
            // Other clients will see
            for (int i = 0; i < createdRooms.Count; i++)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Label(createdRooms[i].Name, GUILayout.Width(400));
                GUILayout.Label(createdRooms[i].PlayerCount + "/" + createdRooms[i].MaxPlayers);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Tham gia"))
                {
                    isJoining = true;
                    PhotonNetwork.NickName = playerName;
                    PhotonNetwork.JoinRoom(createdRooms[i].Name);
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
        //Change NickName
        GUILayout.BeginHorizontal();
        GUILayout.Label("Player Name", GUILayout.Width(85));
        playerName = GUILayout.TextField(playerName, GUILayout.Width(250));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        if (isJoining)
        {
            GUI.enabled = true;
            GUI.Label(new Rect(900 / 2 - 50, 400 / 2 - 10, 100, 20), "Connecting...");
        }
    }
    // Join Room Failed
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        isJoining = false;
        Debug.Log(returnCode + message);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Joined");
        PhotonNetwork.NickName = playerName;
        // Load scene to player
        PhotonNetwork.LoadLevel(sceneName);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("We have received the Room list");
        //After this callback, update the room list
        createdRooms = roomList;
    }
}