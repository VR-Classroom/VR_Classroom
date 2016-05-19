using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateClassMenu : MonoBehaviour
{

    public InputField courseName;
    public InputField daysTaught;

    public Button createClass;

    public Dropdown ClassSize;

    string realDays;
    PlayerInfo p;
    int maxEnroll;

    // Use this for initialization
    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));

        courseName = courseName.GetComponent<InputField>();
        daysTaught = daysTaught.GetComponent<InputField>();
        ClassSize = ClassSize.GetComponent<Dropdown>();

        createClass = createClass.GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {
        if (daysTaught.isFocused == true)
        {
            daysTaught.GetComponent<Image>().color = Color.white;
        }
        else if (courseName.isFocused == true)
        {
            courseName.GetComponent<Image>().color = Color.white;
        }

    }

    public void createNewClass()
    {
        string cn = courseName.text.Trim();
        string dy = daysTaught.text.Trim();
        maxEnroll = int.Parse(ClassSize.options[ClassSize.value].text.Trim());

        if (cn.Length == 0)
        {
            courseName.GetComponent<Image>().color = Color.red;
            return;
        }
        else if (dy.Length == 0)
        {
            daysTaught.GetComponent<Image>().color = Color.red;
            return;
        }

        dy = dy.Replace("(", "");
        dy = dy.Replace(")", "");
        if (dy.Length > 9)
        {
            daysTaught.GetComponent<Image>().color = Color.red;
        }
        string[] days = dy.Split(',');
        string[] valid = { "M", "T", "W", "R", "F", "SA", "SU" };

        bool isValidDay = false;
        for (int i = 0; i < days.Length; ++i)
        {
            isValidDay = false;
            for (int j = 0; j < valid.Length; ++j)
            {
                if (days[i].ToUpper() == valid[j])
                {
                    isValidDay = true;
                    break;
                }
            }
            if (!isValidDay)
            {
                realDays = null;
                Debug.Log(days[i] + " is not a valid day!");
                daysTaught.GetComponent<Image>().color = Color.red;
                return;
            }
            else
            {
                realDays += days[i].ToUpper() + ",";
            }
        }
        realDays = realDays.Substring(0, realDays.Length - 1);
        StartCoroutine(sendNewClass());

    }

    IEnumerator sendNewClass()
    {


        WWWForm form = new WWWForm();

        form.AddField("CNAME", courseName.text.Trim());
        form.AddField("UID", p.uid);
        form.AddField("DAYS", realDays);
        form.AddField("MAXENROLL", maxEnroll);

        WWW download = new WWW(RequestHelper.URL_CREATE_CLASS, form);

        // Wait until the download is done
        yield return download;
        SceneManager.LoadScene("ClassesMenu");
        //if (!string.IsNullOrEmpty(download.error))
        //{
        //    print("Error downloading: " + download.error);
        //}
        //else {
        //    string data = download.text;
        //    if (data == null || data.Trim() == "")
        //    {
        //        Debug.Log("Invalid input");
        //    }
        //    else {
        //        SceneManager.LoadScene("ClassesMenu");
        //    }
        //}
    }

}
