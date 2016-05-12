using UnityEngine;
using System.Collections;

public class Change_Mesh_Render : MonoBehaviour {

    public string url = "https://petersonsbackup.blob.core.windows.net/static/cdn/profiles/10053560/4c686f1c-9b71-4dd0-b46d-f081b1b30ad3";
    private int count = 0;
    int current = 0;
    public string slideName;
    PlayerInfo p;
    public GameObject network;

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

    public void updateClients(string name)
    {
        url = name;
        StartCoroutine(startUpdate());
    }

    public IEnumerator startUpdate()
    {
        WWW www = new WWW(url);
        yield return www;
        GetComponent<Renderer>().material.mainTexture = www.texture;
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
            network.GetComponent<UpdateMeshVarriable>().changePPT("http://52.38.66.127/users/" + p.uid + "/" + slideName + "/p-" + current + ".jpg");
        }
    }

    public void intitSlides(string name)
    {
        StartCoroutine(runInitSlides(name));
        network.GetComponent<UpdateMeshVarriable>().changePPT(url);
    }

    public IEnumerator prevSlidePullup()
    {
        if (current != 0)
        {
            --current;
            WWW www = new WWW("http://52.38.66.127/users/" + p.uid + "/" + slideName + "/p-" + current + ".jpg");
            yield return www;
            GetComponent<Renderer>().material.mainTexture = www.texture;
            network.GetComponent<UpdateMeshVarriable>().changePPT("http://52.38.66.127/users/" + p.uid + "/" + slideName + "/p-" + current + ".jpg");
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
        url = "http://52.38.66.127/users/" + p.uid + "/" + slideName + "/p-" + current + ".jpg";
        //Debug.Log("http://52.38.66.127/users/" + p.uid + "/" + slideName + "/p-" + current + ".jpg");
        WWW www = new WWW("http://52.38.66.127/users/" + p.uid + "/" + slideName + "/p-" + current + ".jpg");
        yield return www;
        GetComponent<Renderer>().material.mainTexture = www.texture;
        count = 8;
        current = 0;
    }
}
