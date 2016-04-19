using UnityEngine;
using System.Collections;

public class Raycast_Hit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        //debug
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 20;
        Debug.DrawRay(transform.position, (forward), Color.green);

        if (Physics.Raycast(transform.position, (forward), out hit))
        {
            hit.transform.SendMessage("HitByRaycast", gameObject,
                              SendMessageOptions.DontRequireReceiver);
        }
    }
}
