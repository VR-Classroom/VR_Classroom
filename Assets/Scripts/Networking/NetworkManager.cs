using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    const string VERSION = "v0.1.0";
    public string roomName = "TEST";
    public string prefabName = "User";
    public Transform[] spawnPoints;
    [SerializeField]
    public Transform TeacherspawnPoints;
    int numPlayers;

    public delegate void OnCharacterInstantiated(GameObject character);

    public static event OnCharacterInstantiated CharacterInstantiated;

    PlayerInfo p;


    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        if (gos.Length == 0)
        {
            SceneManager.LoadScene("LoginMenu");
        }

        p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));
        startConnection(p.roomJoin);

        //else
        //    PhotonNetwork.ConnectUsingSettings(VERSION);
    }
    void startConnection(string room)
    {
        roomName = room;
        numPlayers = PhotonNetwork.playerList.Length;
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
        //>>>>>>> development
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
        if (p.privilege == "T")
        {
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("spawnTeacher", 0);
            Transform t = TeacherspawnPoints;


            GameObject myplayer = PhotonNetwork.Instantiate(prefabName, t.position, t.rotation, 0);
            if (myplayer)
            {

                ExitGames.Client.Photon.Hashtable canTalk = new ExitGames.Client.Photon.Hashtable();
                canTalk.Add(p.uid, p.canTalk);
                PhotonNetwork.room.SetCustomProperties(canTalk);


                myplayer.GetComponent<PlayerInfo>().uid = p.uid;
                myplayer.GetComponent<PlayerInfo>().canTalk = p.canTalk;
                myplayer.GetComponent<PlayerInfo>().fname = p.fname;
                CharacterInstantiated(myplayer);
            }
            PhotonNetwork.room.SetCustomProperties(h);
            ExitGames.Client.Photon.Hashtable playeraAdd = new ExitGames.Client.Photon.Hashtable();
            playeraAdd.Add("myspawn", 100);
            PhotonNetwork.player.SetCustomProperties(playeraAdd);
        }
        else
        {
            int i = 0;
            int[] usedSpawns = new int[spawnPoints.Length];
            for (i = 0; i < usedSpawns.Length; ++i)
            {
                usedSpawns[i] = -1;
            }
            ExitGames.Client.Photon.Hashtable tmp = PhotonNetwork.room.customProperties;
            i = 0;
            int j = 0;
            foreach (var player in PhotonNetwork.playerList)
            {
                if (player.customProperties["myspawn"] != null && (int)player.customProperties["myspawn"] != (int)100)
                {
                    j = (int)player.customProperties["myspawn"];
                    usedSpawns[j] = j;
                }
            }
            bool unavailable = false;
            for (i = 0; i < usedSpawns.Length; ++i)
            {
                unavailable = false;
                for (j = 0; j < usedSpawns.Length; ++j)
                {
                    if (i == usedSpawns[j])
                    {
                        unavailable = true;
                    }
                }
                if (!unavailable)
                    break;
            }
            if(unavailable)
            {
                SceneManager.LoadScene("ClassroomFull");
            }
            Transform t = spawnPoints[i];


            GameObject myplayer = PhotonNetwork.Instantiate(prefabName, t.position, t.rotation, 0);
            if (myplayer)
            {

                ExitGames.Client.Photon.Hashtable canTalk = new ExitGames.Client.Photon.Hashtable();
                canTalk.Add(p.uid, p.canTalk);
                PhotonNetwork.room.SetCustomProperties(canTalk);


                myplayer.GetComponent<PlayerInfo>().uid = p.uid;
                myplayer.GetComponent<PlayerInfo>().canTalk = p.canTalk;
                myplayer.GetComponent<PlayerInfo>().fname = p.fname;
                CharacterInstantiated(myplayer);
            }
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("spawnPlayer" + i, i);
            PhotonNetwork.room.SetCustomProperties(h);
            ExitGames.Client.Photon.Hashtable playeraAdd = new ExitGames.Client.Photon.Hashtable();
            playeraAdd.Add("myspawn", i);
            PhotonNetwork.player.SetCustomProperties(playeraAdd);
        }
    }

    void Update()
    {
        if (numPlayers != PhotonNetwork.playerList.Length)
        {
            int i = 0;
            int[] usedSpawns = new int[spawnPoints.Length];
            for (i = 0; i < usedSpawns.Length; ++i)
            {
                usedSpawns[i] = -1;
            }
            int j = 0;
            foreach (var player in PhotonNetwork.playerList)
            { 
                if (player.customProperties["myspawn"] != null && (int)player.customProperties["myspawn"] != 100)
                {
                    j = (int)player.customProperties["myspawn"];
                    usedSpawns[j] = j;
                }
            }
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            for (i = 0; i < usedSpawns.Length; ++i)
            {
                h.Add("spawnPlayer" + i, usedSpawns[i]);
            }
            PhotonNetwork.room.SetCustomProperties(h);
            numPlayers = PhotonNetwork.playerList.Length;
        }
    }

}
