using UnityEngine;
using System.Collections;

public class Raycasthit : MonoBehaviour {

// Use this for initialization
void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        var cameraCenter = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, GetComponent<Camera>().nearClipPlane));
        if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 1000))
        {
            var obj = hit.transform.gameObject;
        }
    }
}
