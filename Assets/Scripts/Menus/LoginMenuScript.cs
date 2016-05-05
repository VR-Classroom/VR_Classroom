using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginMenuScript : MonoBehaviour
{


    public Button Login;
    public Button Register;
    public Button Exit;

    public InputField username;
    public InputField pass;

    private string passTxt;
    private string emailTxt;

    public Canvas exitMenu;
    public Button yesExit;
    public Button noExit;

    private bool displayExit = false;

    private string loginurl = "http://52.38.66.127/scripts/getUser.php";

    private bool validLogin = false;
    private float checkRate = 1.0f;
    //private MyWebRequest mwr = new MyWebRequest();


    // Use this for initialization
    void Start()
    {
        Login = Login.GetComponent<Button>();
        Register = Register.GetComponent<Button>();
        Exit = Exit.GetComponent<Button>();

        username = username.GetComponent<InputField>();
        pass = pass.GetComponent<InputField>();

        exitMenu = exitMenu.GetComponent<Canvas>();
        yesExit = yesExit.GetComponent<Button>();
        noExit = noExit.GetComponent<Button>();

        switchCanvis();


        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");


        //Debug.Log("gosLength:" + gos.Length);
        for (int i = 0; i < gos.Length - 1; ++i)
        {
            Destroy(gos[i]);
            if (i == gos.Length - 2)
            {
                //Debug.Log("ResetingOne:");
                PlayerInfo p = (PlayerInfo)gos[i + 1].GetComponent(typeof(PlayerInfo));
                p.resetInfo();
            }
        }
    }

    public void LoginPress()
    {
        emailTxt = username.text.Trim();
        passTxt = pass.text.Trim();
        if (passTxt != "" && emailTxt != "")
        {
            //Debug.Log("Calling waitcheck");
            StartCoroutine(waitCheck());
        }
    }


    public void RegisterPress()
    {
        SceneManager.LoadScene("RegisterMenu");
    }

    public void ExitPress()
    {
        switchCanvis();
    }

    public void NoPress()
    {
        switchCanvis();
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    void switchCanvis()
    {
        //main login
        Login.enabled = !displayExit;
        Register.enabled = !displayExit;
        Exit.enabled = !displayExit;
        username.enabled = !displayExit;
        pass.enabled = !displayExit;

        //ensure user wants to exit display
        exitMenu.enabled = displayExit;
        yesExit.enabled = displayExit;
        noExit.enabled = displayExit;

        displayExit = !displayExit;
    }



    IEnumerator waitCheck()
    {


        Debug.Log("Creating form!");
        WWWForm form = new WWWForm();
        //form.AddField("EMAIL", "hgarc014@ucr.edu");
        //    form.AddField("PASS", "1234");



        form.AddField("EMAIL", username.text);
        form.AddField("PASS", pass.text);

        WWW download = new WWW(loginurl, form);

        // Wait until the download is done
        yield return download;

        if (!string.IsNullOrEmpty(download.error))
        {
            print("Error downloading: " + download.error);
        }
        else {
            string data = download.text;
            if (data == null || data.Trim() == "")
            {
                Debug.Log("Invalid input");
            }
            else {
                //string[] rows = data.Split(';');
                //Debug.Log(GetValue(rows[0], "email"));
                //Debug.Log(download.text);
                GameObject t = GameObject.Find("PlayerInfo");
                PlayerInfo p = (PlayerInfo)t.GetComponent(typeof(PlayerInfo));
                p.initPlayer(data);
                Debug.Log("Valid User. TODO: save data before moving to new scene");

                //SceneManager.LoadScene("classroom");
                SceneManager.LoadScene("ClassesMenu");
            }
        }
    }

}
