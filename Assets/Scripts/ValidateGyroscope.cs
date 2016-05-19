using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ValidateGyroscope : MonoBehaviour {

    public Canvas errorCanvas;

	// Use this for initialization
	void Start () {
        errorCanvas = errorCanvas.GetComponent<Canvas>();
        if (Application.isEditor)
        {
            errorCanvas.enabled = false;
        }

        else if (Input.gyro.enabled)
        {
            errorCanvas.enabled = false;
        }
	}

    public void okayPress()
    {
        errorCanvas.enabled = false;
        //SceneManager.LoadScene("ClassesMenu");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
