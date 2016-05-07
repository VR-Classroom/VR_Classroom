﻿using UnityEngine;
using System.Collections;

public class RequestHelper : MonoBehaviour {

    public static string URL_LOGIN = "http://52.38.66.127/scripts/getUser.php";
    public static string URL_REGISTER= "http://52.38.66.127/scripts/registerUser.php";
    public static string URL_GET_CLASSES= "http://52.38.66.127/scripts/getClasses.php";

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static string GetValue(string row, string name)
    {
        if (row == null || name == null)
            Debug.Log("null passed");
        string value = row.Substring(row.IndexOf(name + ":") + name.Length + 1);
        if (value == null)
            Debug.Log("null when parsing " + name);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }
}
