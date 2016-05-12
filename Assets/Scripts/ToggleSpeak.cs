
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToggleSpeak : MonoBehaviour
{

    bool hitbyraycast = false;

    private Color  oldMaterial;

    PlayerInfo p;

    // Use this for initialization
    void Start()
    {
        oldMaterial= gameObject.GetComponent<Renderer>().material.color;
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));
    }

    void HitByRaycast()
    {
        hitbyraycast = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (p.privilege == "T")
        {

            if (hitbyraycast && Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlayerInfo otherPlayer = gameObject.GetComponent<PlayerInfo>();
                otherPlayer.canTalk = !otherPlayer.canTalk;

                ExitGames.Client.Photon.Hashtable tmp = PhotonNetwork.room.customProperties;
                ExitGames.Client.Photon.Hashtable canTalk = new ExitGames.Client.Photon.Hashtable();
                canTalk.Add(otherPlayer.uid, otherPlayer.canTalk);
                PhotonNetwork.room.SetCustomProperties(canTalk);
            }

            if (hitbyraycast)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = oldMaterial;
            }
            hitbyraycast = false;
        }
    }
}
