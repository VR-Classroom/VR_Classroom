using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{

    const string VERSION = "v0.0.6";
    public string roomName = "TEST";
    public string prefabName = "User";
    private bool startCheck = false;
    public Transform[] spawnPoints;
    [SerializeField]
    public Transform TeacherspawnPoints;


    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        if (gos.Length == 0)
        {
            SceneManager.LoadScene("LoginMenu");
        }

        PlayerInfo p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));
        startConnection(p.roomJoin);

        //else
        //    PhotonNetwork.ConnectUsingSettings(VERSION);
    }
    void startConnection(string room)
    {
        roomName = room;
        PhotonNetwork.ConnectUsingSettings(VERSION);
    }

    void OnJoinedLobby()
    {
        RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 10 };
        Debug.Log("Joined lobby for room: " + roomName);
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        startCheck = true;
        //TODO: figure out how to spawn players in order not randomly
        GameObject SceenCamera = GameObject.Find("SceenCamera");
        if (SceenCamera != null && SceenCamera.activeSelf)
        {
            if (SceenCamera != null)
            {
                Debug.Log("SceenCamera not null");
                SceenCamera.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("SceenCamera null");
            }
        }
        int i = 0;
        int[] usedSpawns = new int[10];
        for (i = 0; i < usedSpawns.Length; ++i)
        {
            usedSpawns[i] = -1;
        }
        ExitGames.Client.Photon.Hashtable tmp = PhotonNetwork.room.customProperties;
        i = 0;
        foreach (string name in tmp.Keys)
        {
            usedSpawns[i] = ((int)(tmp[name]));
            //Debug.Log(usedSpawns[i]);
            ++i;
        }
        for (i = 0; i < usedSpawns.Length; ++i)
        {
            bool unavailable = false;
            for (int j = 0; j < usedSpawns.Length; ++j)
            {
                if (i == usedSpawns[j])
                {
                    unavailable = true;
                }
            }
            if (!unavailable)
                break;
        }
        Transform t = spawnPoints[i];
        PhotonNetwork.Instantiate(prefabName, t.position, t.rotation, 0);
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("spawnPlayer" + i, i);
        PhotonNetwork.room.SetCustomProperties(h);
        ExitGames.Client.Photon.Hashtable playeraAdd = new ExitGames.Client.Photon.Hashtable();
        playeraAdd.Add("myspawn", i);
        PhotonNetwork.player.SetCustomProperties(playeraAdd);
    }

    void Update()
    {
        if (startCheck)
        {
            int i = 0;
            int[] usedSpawns = new int[8];
            for (i = 0; i < usedSpawns.Length; ++i)
            {
                usedSpawns[i] = -1;
            }
            int j = 0;
            foreach (var player in PhotonNetwork.playerList)
            {
                j = (int)player.customProperties["myspawn"];
                usedSpawns[j] = j;
            }
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            for (i = 0; i < usedSpawns.Length; ++i)
            {
                h.Add("spawnPlayer" + i, usedSpawns[i]);
            }
            PhotonNetwork.room.SetCustomProperties(h);
        }
    }

}
