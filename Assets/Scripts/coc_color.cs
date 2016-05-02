using UnityEngine;

using System.Collections;



public class coc_color : MonoBehaviour {


	public static coc_color instance;
	public int mycolor;
	// Use this for initialization
	
	void Start () {

		mycolor = 0;
		instance = this;
	}
	
	
	// Update is called once per frame
	
	void Update () {

	
	if (mycolor == 1){
			gameObject.GetComponent<Renderer>().material.color = Color.red;
		}
		if (mycolor == 2){
			gameObject.GetComponent<Renderer>().material.color = Color.blue;
		}
	}

}
