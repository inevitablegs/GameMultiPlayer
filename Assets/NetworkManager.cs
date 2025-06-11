using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Connect();
    }

    private void Awake()
    {
        // Initialize Photon
        PhotonNetwork.AutomaticallySyncScene = true; // Automatically sync scene changes
    }

    public void Connect()
    {
        // Connect to Photon server
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Play()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // If joining a random room fails, create a new room
        Debug.Log("Join Random Room Failed: " + message);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        // Called when successfully joined a room
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        if (PhotonNetwork.IsMasterClient)
        {
            // If this client is the master client, instantiate a player prefab
            PhotonNetwork.LoadLevel(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
