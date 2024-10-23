using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyController : MonoBehaviourPunCallbacks
{

  // Tên người chơi của chúng ta
  string playerName = "Player 1";
  // Phiên bản game (cho phép thực hiện các thay đổi đột phá mà không làm hỏng game của những người chơi khác dùng phiên bản cũ)
  string gameVersion = "1.0";
  // Danh sách các phòng đã tạo
  List<RoomInfo> createdRooms = new List<RoomInfo>();
  // Tên phòng để tạo
  string roomName = "Room 1";
  Vector2 roomListScroll = Vector2.zero; // Vị trí cuộn của danh sách phòng
  bool joiningRoom = false; // Kiểm tra xem đang trong quá trình gia nhập phòng hay không


  // Start 
  void Start()
  {
    // Khởi tạo tên người chơi ngẫu nhiên
    playerName = "Player " + Random.Range(111, 999);

    // Bật tính năng đồng bộ cảnh tự động
    PhotonNetwork.AutomaticallySyncScene = true;

    // Kiểm tra kết nối Photon, nếu chưa kết nối thì thực hiện kết nối với server theo thiết lập mặc định
    if (!PhotonNetwork.IsConnected)
    {
      // Thiết lập phiên bản ứng dụng trước khi kết nối
      PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
      PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu"; // Thiết lập vùng server cố định (tùy chọn)
      PhotonNetwork.ConnectUsingSettings(); // Kết nối tới Photon server
    }
  }

  // OnDisconnected (Được gọi khi mất kết nối với Photon server)
  public override void OnDisconnected(DisconnectCause cause)
  {
    Debug.Log("Mất kết nối với Photon server. Mã lỗi: " + cause.ToString() + " Địa chỉ server: " + PhotonNetwork.ServerAddress);
  }

  // OnConnectedToMaster (Được gọi khi kết nối thành công với Master server)
  public override void OnConnectedToMaster()
  {
    Debug.Log("Đã kết nối tới Master server");
    // Gia nhập lobby mặc định
    PhotonNetwork.JoinLobby(TypedLobby.Default);
  }

  // OnRoomListUpdate (Được gọi khi nhận được danh sách phòng)
  public override void OnRoomListUpdate(List<RoomInfo> roomList)
  {
    Debug.Log("Đã nhận được danh sách phòng");
    // Cập nhật danh sách phòng đã tạo với danh sách mới
    createdRooms = roomList;
  }

  // OnGUI (Được gọi để hiển thị các thành phần giao diện người dùng)
  void OnGUI()
  {
    // Tạo một cửa sổ GUI tên "Lobby"
    GUI.Window(0, new Rect(Screen.width / 2 - 450, Screen.height / 2 - 200, 900, 400), LobbyWindow, "Lobby");
  }

  // LobbyWindow (Hàm để thiết kế giao diện của cửa sổ Lobby)
  void LobbyWindow(int index)
  {
    // Hiển thị trạng thái kết nối và nút tạo phòng
    GUILayout.BeginHorizontal();

    GUILayout.Label("Trạng thái: " + PhotonNetwork.NetworkClientState); // Hiển thị trạng thái kết nối hiện tại

    // Kiểm tra nếu đang kết nối, đang gia nhập phòng hoặc chưa kết nối lobby thì nút sẽ bị vô hiệu hóa
    if (joiningRoom || !PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
    {
      GUI.enabled = false;
    }

    GUILayout.FlexibleSpace(); // Thêm khoảng trống linh hoạt

    // Cho phép nhập tên phòng và nút tạo phòng
    roomName = GUILayout.TextField(roomName, GUILayout.Width(250)); // Hiển thị ô nhập tên phòng

    if (GUILayout.Button("Tạo phòng", GUILayout.Width(125))) // Nút tạo phòng
    {
      if (roomName != "") // Kiểm tra nếu tên phòng trống thì không thực hiện tạo phòng
      {
        joiningRoom = true;

        RoomOptions roomOptions = new RoomOptions(); // Thiết lập các tùy chọn phòng
        roomOptions.IsOpen = true; // Phòng mở (cho phép người khác tham gia)
        roomOptions.IsVisible = true; // Phòng hiển thị trong danh sách
        roomOptions.MaxPlayers = (byte)10; // Số người chơi tối đa (có thể thay đổi)

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default); // Tạo hoặc tham gia phòng với tên đã nhập
      }
    }

    GUILayout.EndHorizontal();

    // Duyệt qua danh sách phòng có thể tham gia
    roomListScroll = GUILayout.BeginScrollView(roomListScroll, true, true); // Khởi tạo thanh cuộn danh sách phòng

    if (createdRooms.Count == 0) // Kiểm tra nếu không có phòng nào
    {
      GUILayout.Label("Hiện chưa có phòng nào...");
    }
    else
    {
      for (int i = 0; i < createdRooms.Count; i++) // Lặp qua từng phòng trong danh sách
      {
        GUILayout.BeginHorizontal("box"); // Khung nền cho mỗi phòng

        GUILayout.Label(createdRooms[i].Name, GUILayout.Width(400)); // Hiển thị tên phòng
        GUILayout.Label(createdRooms[i].PlayerCount + "/" + createdRooms[i].MaxPlayers); // Hiển thị số người chơi hiện tại/tối đa

        GUILayout.FlexibleSpace(); // Thêm khoảng trống linh hoạt

        if (GUILayout.Button("Tham gia phòng")) // Nút tham gia phòng
        {
          joiningRoom = true;

          // Thiết lập tên người chơi
          PhotonNetwork.NickName = playerName;

          // Tham gia phòng với tên đã chọn
          PhotonNetwork.JoinRoom(createdRooms[i].Name);
        }
        GUILayout.EndHorizontal();
      }
    }

    GUILayout.EndScrollView(); // Kết thúc thanh cuộn danh sách phòng

    // Hiển thị ô nhập tên người chơi và nút làm mới danh sách phòng
    GUILayout.BeginHorizontal();

    GUILayout.Label("Tên người chơi: ", GUILayout.Width(85));
    //Player name text field
    playerName = GUILayout.TextField(playerName, GUILayout.Width(250)); // Hiển thị ô nhập tên người chơi

    GUILayout.FlexibleSpace(); // Thêm khoảng trống linh hoạt

    // Kiểm tra trạng thái kết nối và không đang gia nhập phòng thì nút làm mới mới hoạt động
    GUI.enabled = (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby || PhotonNetwork.NetworkClientState == ClientState.Disconnected) && !joiningRoom;
    if (GUILayout.Button("Làm mới", GUILayout.Width(100))) // Nút làm mới danh sách phòng
    {
      if (PhotonNetwork.IsConnected) // Kiểm tra nếu đã kết nối
      {
        // Tham gia lại lobby để lấy danh sách phòng mới nhất
        PhotonNetwork.JoinLobby(TypedLobby.Default);
      }
      else // Nếu chưa kết nối thì thiết lập kết nối mới
      {
        PhotonNetwork.ConnectUsingSettings();
      }
    }

    GUILayout.EndHorizontal();

    // Hiển thị thông báo "Đang kết nối..." khi đang trong quá trình gia nhập phòng
    if (joiningRoom)
    {
      GUI.enabled = true; // Bật giao diện hiển thị thông báo
      GUI.Label(new Rect(900 / 2 - 50, 400 / 2 - 10, 100, 20), "Đang kết nối..."); // Hiển thị thông báo
    }
  }

  // OnCreateRoomFailed (Được gọi khi tạo phòng thất bại)
  public override void OnCreateRoomFailed(short returnCode, string message)
  {
    Debug.Log("Tạo phòng thất bại. Mã lỗi: " + returnCode + ". Thông báo: " + message + ". Có thể phòng đã tồn tại (ngay cả khi không hiển thị). Hãy thử tên phòng khác.");
    joiningRoom = false; // Xóa cờ đang gia nhập phòng
  }

  // OnJoinRoomFailed (Được gọi khi tham gia phòng thất bại)
  public override void OnJoinRoomFailed(short returnCode, string message)
  {
    Debug.Log("Tham gia phòng thất bại. Mã lỗi: " + returnCode + ". Thông báo: " + message + ". Có thể phòng không tồn tại, đã đầy hoặc đóng.");
    joiningRoom = false; // Xóa cờ đang gia nhập phòng
  }

  // OnJoinRandomFailed (Được gọi khi tham gia phòng ngẫu nhiên thất bại - Ít sử dụng trong code này)
  public override void OnJoinRandomFailed(short returnCode, string message)
  {
    Debug.Log("Tham gia phòng ngẫu nhiên thất bại. Mã lỗi: " + returnCode + ". Thông báo: " + message + ". Có thể phòng không tồn tại, đã đầy hoặc đóng.");
    joiningRoom = false; // Xóa cờ đang gia nhập phòng
  }

  // OnCreatedRoom (Được gọi khi tạo phòng thành công)
  public override void OnCreatedRoom()
  {
    Debug.Log("Đã tạo phòng");
    // Thiết lập tên người chơi
    PhotonNetwork.NickName = playerName;
    // Tải cảnh chơi có tên Playground (Đảm bảo cảnh chơi đã được thêm vào build settings)
    PhotonNetwork.LoadLevel("Playground");
  }

  // OnJoinedRoom (Được gọi khi tham gia phòng thành công)
  public override void OnJoinedRoom()
  {
    Debug.Log("Đã tham gia phòng");
  }
}