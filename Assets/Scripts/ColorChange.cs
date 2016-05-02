using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ColorChange : MonoBehaviour {

    bool hitbyraycast = false;

	// Use this for initialization
	void Start () {
	
	}

    void HitByRaycast()
    {
        //Debug.Log("I was hit by a Ray");
        hitbyraycast = true;
    }

    // Update is called once per frame
    void Update () {
        if (hitbyraycast && Input.GetKey(KeyCode.Mouse0))
        {
            ExitGames.Client.Photon.Hashtable tmp = PhotonNetwork.player.customProperties;
            int i = (int)(tmp["myspawn"]);
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("spawnPlayer" + i, -1);
            PhotonNetwork.room.SetCustomProperties(h);
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);

            PhotonNetwork.Disconnect();

            SceneManager.LoadScene("LoginMenu");
            //Application.Quit();
            //Debug.Log("exiting");
        }

        if (hitbyraycast)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.black;
        }
        hitbyraycast = false;
    }
}
