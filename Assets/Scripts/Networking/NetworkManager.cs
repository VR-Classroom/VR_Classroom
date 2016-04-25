using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    const string VERSION = "v0.0.1";
    public string roomName = "TEST";
    public string prefabName = "User";
    public Transform[] spawnPoints;
    GameObject[] test;

	void Start () {
        PhotonNetwork.ConnectUsingSettings(VERSION);
        test[0] = GameObject.Find("SpawnPoint01");
        test[1] = GameObject.Find("SpawnPoint02");
        test[2] = GameObject.Find("SpawnPoint03");
        test[3] = GameObject.Find("SpawnPoint04");
        test[4] = GameObject.Find("SpawnPoint05");
        test[5] = GameObject.Find("SpawnPoint06");
        test[6] = GameObject.Find("SpawnPoint07");
        test[7] = GameObject.Find("SpawnPoint08");
        for(int i = 0; i +1 < test.Length; ++i)
        {
            test[i + 1].SetActive(false);
        }
    }

    void OnJoinedLobby()
    {
        RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        //TODO: figure out how to spawn players in order not randomly
        Transform t = spawnPoints[Random.Range(0, spawnPoints.Length)];
        PhotonNetwork.Instantiate(prefabName, t.position,t.rotation,0);
    }
	
}
