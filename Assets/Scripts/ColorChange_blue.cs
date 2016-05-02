using UnityEngine;
using System.Collections;

public class ColorChange_blue : MonoBehaviour {

    bool hitbyraycast = false;

	// Use this for initialization
	void Start () {
	
	}

    void HitByRaycast()
    {
        hitbyraycast = true;
    }

    // Update is called once per frame
    void Update () { 

        if (hitbyraycast)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;

	    
	    if(Input.GetMouseButtonDown(0)) //4space instead of tab? Are you guys classless?
	    {
		//and your brackets are wrong. They're not even consistent.
		//================ ADD OBJECT ACTION HERE ==================
		Debug.Log("Pressed left click.");
		
		coc_color.instance.mycolor = 2;
		
	    }
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.black;
        }
        hitbyraycast = false;
    }
}
