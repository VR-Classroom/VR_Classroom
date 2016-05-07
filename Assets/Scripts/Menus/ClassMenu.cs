using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClassMenu : MonoBehaviour
{



    //public GameObject prefabButton;
    //public RectTransform ParentPanel;


    // Use this for initialization
    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        if (gos.Length == 0)
        {
            SceneManager.LoadScene("LoginMenu");
        }
        //for (int i = 0; i < 20; i++)
        //{
        //    GameObject goButton = (GameObject)Instantiate(prefabButton);
        //    goButton.transform.SetParent(ParentPanel, false);
        //    goButton.transform.localScale = new Vector3(1, 1, 1);

        //    Button tempButton = goButton.GetComponent<Button>();

        //    int tempInt = i;

        //    tempButton.onClick.AddListener(() => ButtonClicked(tempInt));
        //}

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LogoutPress()
    {

        //GameObject t = GameObject.Find("PlayerInfo");
        ////PlayerInfo p = (PlayerInfo)t.GetComponent(typeof(PlayerInfo));
        //Object.Destroy(t);
        ////p.resetInfo();
        SceneManager.LoadScene("LoginMenu");
    }

    public void ClassPress()
    {
        SceneManager.LoadScene("classroom");
    }



    void ButtonClicked(int buttonNo)
    {
        Debug.Log("Button clicked = " + buttonNo);
    }
}
