using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField roomCodeInput;
    public GameObject uiPanel;
    public Text connectionStatusText;

    void Start()
    {
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        connectionStatusText.text = "Connecting to Photon...";
        Debug.Log("Connecting to Photon...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server.");
        connectionStatusText.text = "Connected to Master";

        // Join the default lobby
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"Disconnected from Photon: {cause}");
        connectionStatusText.text = $"Disconnected: {cause}";

        // Try to reconnect
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        connectionStatusText.text = "Connected and in Lobby";
        uiPanel.SetActive(true); // Show UI panel when ready
    }

    public void CreateRoom()
    {
        string roomName = roomCodeInput.text;
        if (!string.IsNullOrEmpty(roomName))
        {
            RoomOptions options = new RoomOptions { MaxPlayers = 4 };
            PhotonNetwork.CreateRoom(roomName, options);
            connectionStatusText.text = "Creating room...";
        }
    }

    public void JoinRoom()
    {
        string roomName = roomCodeInput.text;
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.JoinRoom(roomName);
            connectionStatusText.text = "Joining room...";
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        connectionStatusText.text = "Joined Room - Loading level...";
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room: " + message);
        connectionStatusText.text = $"Join failed: {message}";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room: " + message);
        connectionStatusText.text = $"Create failed: {message}";
    }

}