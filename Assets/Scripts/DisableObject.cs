using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DisableObject : NetworkBehaviour
{

	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
        {
            Debug.Log("disabling cardboard components");
            //gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
