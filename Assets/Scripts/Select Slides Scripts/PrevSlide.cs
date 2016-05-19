using UnityEngine;
using System.Collections;

public class PrevSlide : MonoBehaviour {

    bool hitbyraycast = false;
    public GameObject []projectors;
    PlayerInfo p;

    // Use this for initialization
    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));
        if (p.privilege != "T")
        {
            gameObject.SetActive(false);
        }
    }

    void HitByRaycast()
    {
        //Debug.Log("I was hit by a Ray");
        hitbyraycast = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hitbyraycast && Input.GetKeyDown(KeyCode.Mouse0))
        {
            foreach(GameObject projector in projectors)
            projector.GetComponent<Change_Mesh_Render>().prevSlide();
        }

        if (hitbyraycast)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
        hitbyraycast = false;
    }
}
