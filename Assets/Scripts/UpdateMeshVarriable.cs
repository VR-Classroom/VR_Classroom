using UnityEngine;
using System.Collections;

public class UpdateMeshVarriable : MonoBehaviour {

    public GameObject[] projectors;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject projector in projectors)
        {
            if (projector.GetComponent<Change_Mesh_Render>().url != (string)PhotonNetwork.room.customProperties["slideName"] && (string)PhotonNetwork.room.customProperties["slideName"] != null)
            {
                projector.GetComponent<Change_Mesh_Render>().updateClients((string)PhotonNetwork.room.customProperties["slideName"]);
            }
        }
	}

    public void changePPT(string name)
    {
        //Debug.Log(name);
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("slideName", name);
        PhotonNetwork.room.SetCustomProperties(h);
    }
}
