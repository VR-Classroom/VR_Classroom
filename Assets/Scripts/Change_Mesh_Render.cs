using UnityEngine;
using System.Collections;

public class Change_Mesh_Render : MonoBehaviour {

    private string url = "https://petersonsbackup.blob.core.windows.net/static/cdn/profiles/10053560/4c686f1c-9b71-4dd0-b46d-f081b1b30ad3";
    private int count = 0;
    int current = 0;
    string slideName;
    PlayerInfo p;

    // Use this for initialization
    IEnumerator Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));
        WWW www = new WWW(url);
        yield return www;
        GetComponent<Renderer>().material.mainTexture = www.texture;
    }

    // Update is called once per frame
    void Update () {
	    
	}
    public IEnumerator nextSlidePullup()
    {
        if (current + 1 < count)
        {
            ++current;
            //Debug.Log("http://52.38.66.127/users/" + p.uid + "/" + slideName + "/p-" + current + ".jpg");
            WWW www = new WWW("http://52.38.66.127/users/" + p.uid + "/" + slideName + "/p-" + current + ".jpg");
            yield return www;
            GetComponent<Renderer>().material.mainTexture = www.texture;
        }
    }

    public void intitSlides(string name)
    {
        StartCoroutine(runInitSlides(name));
    }

    public IEnumerator prevSlidePullup()
    {
        if (current != 0)
        {
            --current;
            WWW www = new WWW("http://52.38.66.127/users/" + p.uid + "/" + slideName + "/p-" + current + ".jpg");
            yield return www;
            GetComponent<Renderer>().material.mainTexture = www.texture;
        }
    }

    public void nextSlide()
    {
        StartCoroutine(nextSlidePullup());
    }

    public void prevSlide()
    {
        StartCoroutine(prevSlidePullup());
    }

    IEnumerator runInitSlides(string name)
    {
        slideName = name.Trim();
        //Debug.Log("http://52.38.66.127/users/" + p.uid + "/" + slideName + "/p-" + current + ".jpg");
        WWW www = new WWW("http://52.38.66.127/users/" + p.uid + "/" + slideName + "/p-" + current + ".jpg");
        yield return www;
        GetComponent<Renderer>().material.mainTexture = www.texture;
        count = 8;
        current = 0;
    }
}
