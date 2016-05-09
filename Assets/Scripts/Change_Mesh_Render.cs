using UnityEngine;
using System.Collections;

public class Change_Mesh_Render : MonoBehaviour {

    private string url = "https://upload.wikimedia.org/wikipedia/commons/b/b4/JPEG_example_JPG_RIP_100.jpg";
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
	    if (count == 100)
        {
            //url = "file:///C:/Users/Preston/Downloads/star-wars-battleship-destroyer-263526.jpg";
            //WWW www = new WWW(url);
            //GetComponent<Renderer>().material.mainTexture = www.texture;
        }
        count++;
	}
}
