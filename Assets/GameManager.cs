using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public string playerPrefabName = "Player";
    public Vector3[] spawnPoints;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        Debug.Log($"PhotonNetwork.InRoom: {PhotonNetwork.InRoom}");
        Debug.Log($"PlayerPrefab exists: {Resources.Load(playerPrefabName) != null}");
        
        if (PhotonNetwork.InRoom)
        {
            Vector3 spawnPos = spawnPoints.Length > 0
                ? spawnPoints[Random.Range(0, spawnPoints.Length)]
                : new Vector3(0, 1, 0);

            PhotonNetwork.Instantiate(playerPrefabName, spawnPos, Quaternion.identity);
            Debug.Log($"Player instantiated at {spawnPos}");
        }
        else
        {
            Debug.LogWarning("Not in room yet!");
        }
    }

    public override void OnJoinedRoom()
    {
        SpawnPlayer();
    }
}