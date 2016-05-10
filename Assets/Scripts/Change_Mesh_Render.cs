using UnityEngine;
using System.Collections;

public class Change_Mesh_Render : MonoBehaviour {

    private string url = "https://petersonsbackup.blob.core.windows.net/static/cdn/profiles/10053560/4c686f1c-9b71-4dd0-b46d-f081b1b30ad3";
    private int count = 0;

    // Use this for initialization
    IEnumerator Start()
    {
        WWW www = new WWW(url);
        yield return www;
        GetComponent<Renderer>().material.mainTexture = www.texture;
    }

    // Update is called once per frame
    void Update () {
	    
	}
}
