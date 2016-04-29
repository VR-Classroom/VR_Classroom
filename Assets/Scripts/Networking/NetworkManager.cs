﻿using UnityEngine;
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
        for(i = 0; i < usedSpawns.Length; ++i)
        {
            usedSpawns[i] = -1;
        }
        ExitGames.Client.Photon.Hashtable tmp = PhotonNetwork.room.customProperties;
        i = 0;
        foreach (string name in tmp.Keys)
        {
            usedSpawns[i] = ((int)(tmp[name]));
            Debug.Log(usedSpawns[i]);
            ++i;
        }
        for(i = 0; i < usedSpawns.Length; ++i)
        {
            bool unavailable = false;
            for (int j =0; j < usedSpawns.Length; ++j)
            {
                if(i == usedSpawns[j])
                {
                    unavailable = true;
                }
            }
            if (!unavailable)
                break;
        }
        Transform t = spawnPoints[i];
        PhotonNetwork.Instantiate(prefabName, t.position,t.rotation,0);
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("spawnPlayer" + i, i);
        PhotonNetwork.room.SetCustomProperties(h, null, false);
    }
	
}