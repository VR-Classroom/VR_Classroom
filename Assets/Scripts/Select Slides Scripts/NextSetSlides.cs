using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NextSetSlides : MonoBehaviour
{

    public GameObject scriptHolder;

    bool hitbyraycast = false;

    // Use this for initialization
    void Start()
    {

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
            scriptHolder.GetComponent<ShowPPTNames>().Next();
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