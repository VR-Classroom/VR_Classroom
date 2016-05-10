using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowPPTNames : MonoBehaviour {

    PlayerInfo p;
    public GameObject podium;

    // Use this for initialization
    void Start () {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));
        if (p.privilege != "T")
            podium.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator getPPTInfo()
    {

        WWWForm form = new WWWForm();

        GameObject t = GameObject.Find("Title");
        Text title = t.GetComponent<Text>();

        form.AddField("UID", p.uid);

        WWW download = new WWW(RequestHelper.URL_GET_CLASSES, form);

        // Wait until the download is done
        yield return download;

        string text = "presentation1 / presentation2 / presentation3 /";

        if (!string.IsNullOrEmpty(download.error))
        {
            print("Error downloading: " + download.error);
        }
        else
        {
            string data = text;
            if (data == null || data.Trim() == "")
            {
                title.text = "You have no PowerPoints";
                //Debug.Log("Invalid input");
            }
            else
            {
                string[] pptNames = data.Split('/');
                ExitGames.Client.Photon.Hashtable tmp = PhotonNetwork.room.customProperties;
                ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
                if(pptNames.Length != 0)
                    h.Add("ppt1", pptNames[0]);
                if (pptNames.Length >= 1)
                    h.Add("ppt2", pptNames[1]);
                PhotonNetwork.room.SetCustomProperties(h);
            }
        }
    }
}
