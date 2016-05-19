using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkPlayer : Photon.MonoBehaviour{

    public GameObject myCamera;
    public GameObject sphere;
    public Image speaker;
    public Text name;

    PlayerInfo p;

    // Use this for initialization
    void Start()
    {
        if (photonView.isMine)
        {
            myCamera.SetActive(true);
            sphere.GetComponent<FollowCamera>().enabled = true;
            this.gameObject.GetComponent<PhotonVoiceRecorder>().enabled = true;
            name.text = "";
            name = null;
            speaker = null;
            
        }
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));
    }
    void Update()
    {

        if (speaker != null && name != null)
        {
            Camera speakerC = speaker.GetComponent<LookAtCamera>().c;
            Camera nameC = name.GetComponent<LookAtCamera>().c;
            Camera mainc = GameObject.Find("Camera").GetComponent<Camera>();
            if (mainc != null && speakerC != mainc&& nameC != Camera.main)
            {
                //Debug.Log("Updating Main camera for name and speaker");
                speaker.GetComponent<LookAtCamera>().c = mainc;
                name.GetComponent<LookAtCamera>().c = mainc;
                name.text = gameObject.GetComponent<PlayerInfo>().dispName;
            }
        }

        ExitGames.Client.Photon.Hashtable tmpRoom= PhotonNetwork.room.customProperties;
        if (p != null&& tmpRoom != null && tmpRoom[p.uid] != null)
        {
            if (p.canTalk != (bool)tmpRoom[p.uid])
            {
                p.canTalk = (bool)tmpRoom[p.uid];
            }
        }
        

    }
 }
