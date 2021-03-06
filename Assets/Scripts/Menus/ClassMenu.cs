﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClassMenu : MonoBehaviour
{
    public Canvas createClassMenu;
    public Button createClass;
    public Button uploadPpt;

    public Button cancel;
    PlayerInfo p;

    // Use this for initialization
    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        if (gos.Length == 0)
        {
            SceneManager.LoadScene("LoginMenu");
        }

        p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));

        createClassMenu = createClassMenu.GetComponent<Canvas>();
        createClassMenu.enabled = false;

        createClass = createClass.GetComponent<Button>();
        uploadPpt = uploadPpt.GetComponent<Button>();

        if (p.privilege != "T")
        {
            createClass.gameObject.SetActive(false);
            uploadPpt.gameObject.SetActive(false);
        }

        cancel = cancel.GetComponent<Button>();
        cancel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LogoutPress()
    {
        SceneManager.LoadScene("LoginMenu");
    }

    public void uploadPptPress()
    {
        Application.OpenURL(RequestHelper.URL_UPLOAD_PPT);
    }

    public void CreateNewClassPress()
    {
        
        createClassMenu.enabled = true;
        createClass.gameObject.SetActive(false);
        uploadPpt.gameObject.SetActive(false);
        cancel.gameObject.SetActive(true);
    }


    public void cancelCreate()
    {
        createClassMenu.enabled = false;
        createClass.gameObject.SetActive(true);
        uploadPpt.gameObject.SetActive(true);
        cancel.gameObject.SetActive(false);
    }

    public void okPress()
    {
        SceneManager.LoadScene("ClassesMenu");
    }

}
