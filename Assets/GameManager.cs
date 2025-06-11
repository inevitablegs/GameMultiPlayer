using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{

    public GameObject playerPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0,1,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
