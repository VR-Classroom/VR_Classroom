using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour{

    public GameObject myCamera;
    public GameObject sphere;

    // Use this for initialization
    void Start()
    {
        if (photonView.isMine)
        {
            myCamera.SetActive(true);
            sphere.GetComponent<FollowCamera>().enabled = true;

            //GetComponent<FollowCamera>().enabled = true;
            //GetComponent<Raycast_Hit>().enabled = true;
            //GetComponent<Camera>().enabled = true;
        }
        else {
        }
    }
    void Update()
    {
        //ExitGames.Client.Photon.Hashtable tmp = PhotonNetwork.player.customProperties;
        // Debug.Log("I am at spawn point " + tmp["myspawn"]);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGames.Client.Photon.Hashtable tmp = PhotonNetwork.player.customProperties;
            int i = (int)(tmp["myspawn"]);
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("spawnPlayer" + i, -1);
            PhotonNetwork.room.SetCustomProperties(h);
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
            Application.Quit();
        }
    }
 }
