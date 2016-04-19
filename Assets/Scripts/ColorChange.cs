using UnityEngine;
using System.Collections;

public class ColorChange : MonoBehaviour {

    bool hitbyraycast = false;

	// Use this for initialization
	void Start () {
	
	}

    void HitByRaycast()
    {
        Debug.Log("I was hit by a Ray");
        hitbyraycast = true;
    }

    // Update is called once per frame
    void Update () {
        if (hitbyraycast && Input.GetKey(KeyCode.KeypadEnter))
        {
            Debug.Log("exiting");
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
