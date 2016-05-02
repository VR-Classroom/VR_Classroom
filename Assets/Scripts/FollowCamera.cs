using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {


    public Camera c;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.rotation = Quaternion.LookRotation(c.transform.forward, -c.transform.up);
        //transform.Rotate(0, Time.deltaTime * 10, 0, Space.World);
    }
}
