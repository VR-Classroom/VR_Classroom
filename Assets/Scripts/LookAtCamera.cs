using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

    public Camera c;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //transform.LookAt(-c.transform.position, c.transform.up);

        Vector3 v = c.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(c.transform.position - v);
        transform.Rotate(0, 180, 0);

    }
}
