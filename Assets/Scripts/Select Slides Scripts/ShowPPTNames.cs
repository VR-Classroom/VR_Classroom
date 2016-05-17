using UnityEngine;
using System.Collections;

public class ShowPPTNames : MonoBehaviour {

    PlayerInfo p;
    public GameObject[] podium;
    int numNexts = 0;
    int currentPPTnames = 0;
    string[] pptNames;
    string ppt1;
    string ppt2;

    // Use this for initialization
    void Start () {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));
        if (p.privilege != "T")
        {
            for (int i = 0; i < podium.Length; ++i)
            {
                podium[i].gameObject.SetActive(false);
            }
        }
        StartCoroutine(getPPTInfo());
    }

    public void Next()
    {
        if (pptNames.Length > (currentPPTnames + 1))
        {
            ppt1 = pptNames[currentPPTnames + 1];
            ++currentPPTnames;
        }
        else
        {
            return;
        }
        if (pptNames.Length > (currentPPTnames + 1))
        {
            ppt2 = pptNames[currentPPTnames + 1];
            ++currentPPTnames;
        }
        else
        {
            ppt2 = "";
        }
        //Debug.Log(ppt1);
        //Debug.Log(ppt2);
        podium[0].GetComponent<SelectPPT>().UpdateName(ppt1, ppt2);
        podium[1].GetComponent<SelectPPT>().UpdateName(ppt1, ppt2);
    }

    public void prev()
    {
        if (currentPPTnames - 2 <= 0)
        {
            return;
        }
        if (currentPPTnames % 2 == 1)
        {
            ppt2 = pptNames[currentPPTnames - 2];
            --currentPPTnames;
            ppt1 = pptNames[currentPPTnames - 2];
            --currentPPTnames;
        }
        else
        {
            ppt2 = pptNames[currentPPTnames - 1];
            ppt1 = pptNames[currentPPTnames - 2];
            --currentPPTnames;
        }
        //Debug.Log(ppt1);
        //Debug.Log(ppt2);
        podium[0].GetComponent<SelectPPT>().UpdateName(ppt1, ppt2);
        podium[1].GetComponent<SelectPPT>().UpdateName(ppt1, ppt2);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    IEnumerator getPPTInfo()
    {
        WWWForm form = new WWWForm();

        form.AddField("UID", p.uid);

        WWW download = new WWW(RequestHelper.URL_GET_PRESENTATIONS, form);

        // Wait until the download is done
        yield return download;

        //string text = "presentation1 / presentation2 / presentation3 /";

        if (!string.IsNullOrEmpty(download.error))
        {
            print("Error downloading: " + download.error);
        }
        else
        {
            string data = download.text;
            if (data == null || data.Trim() == "")
            {
                //title.text = "You have no PowerPoints";
                //Debug.Log("Invalid input");
            }
            else
            {
                pptNames = data.Split('/');
                if (pptNames.Length != 0)
                {
                    ppt1 = pptNames[0];
                }
                if (pptNames.Length >= 1)
                {
                    ppt2 = pptNames[1];
                    ++currentPPTnames;
                }
                podium[0].GetComponent<SelectPPT>().UpdateName(ppt1, ppt2);
                podium[1].GetComponent<SelectPPT>().UpdateName(ppt1, ppt2);
            }
        }
    }
}
