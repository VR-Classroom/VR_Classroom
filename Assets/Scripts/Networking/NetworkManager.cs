using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    const string VERSION = "v0.0.1";
    public string roomName = "TEST";
    public string prefabName = "User";
    public Transform[] spawnPoints;

	void Start () {
        PhotonNetwork.ConnectUsingSettings(VERSION);
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
