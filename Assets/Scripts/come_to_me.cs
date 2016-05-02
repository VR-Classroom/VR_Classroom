using UnityEngine;

using System.Collections;


	public class come_to_me : MonoBehaviour {


	int startup_wait;
	GameObject ScreenCamera;
	// Use this for initialization
	
	void Start () {

	
	startup_wait = 125;
		
	}
	
	
	// Update is called once per frame
	
	void Update () {
		if (startup_wait != 0){
			startup_wait = startup_wait - 1;
			if (startup_wait == 0){
				ScreenCamera = GameObject.Find("User(Clone)/Sphere/CardboardHead");
				transform.position = ScreenCamera.transform.position + (ScreenCamera.transform.forward);
			}
		}else{
			
		}
		
	}

}
