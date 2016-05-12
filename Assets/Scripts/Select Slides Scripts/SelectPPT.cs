using UnityEngine;
using System.Collections;

public class SelectPPT : MonoBehaviour {

    bool hitbyraycast = false;
    public GameObject Nameppt;
    public GameObject projector;

    // Use this for initialization
    void Start()
    {

    }

    void HitByRaycast()
    {
        //Debug.Log("I was hit by a Ray");
        hitbyraycast = true;
    }

    public void UpdateName(string ppt1, string ppt2)
    {
        if (this.gameObject.name == "ppt1")
        {
            Nameppt.GetComponent<TextMesh>().text = ppt1;
        }
        else
            Nameppt.GetComponent<TextMesh>().text = ppt2;
    }

    // Update is called once per frame
    void Update()
    {
        if (hitbyraycast && Input.GetKeyDown(KeyCode.Mouse0))
        {
            projector.GetComponent<Change_Mesh_Render>().intitSlides(Nameppt.GetComponent<TextMesh>().text);
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
