using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollableList : MonoBehaviour
{
    public GameObject itemPrefab;
    //public int itemCount = 1;
    public int columnCount = 1;
    public Scrollbar scrollBar;
    public Canvas ExitMenu;
    public Text ExitTitle;

    PlayerInfo p;
    int cid = -1;


    void createButtons(List<ClassInfo> classes)
    {
        RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform>();
        RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();


        int itemCount = classes.Count;

        //calculate the width and height of each child item.
        float width = containerRectTransform.rect.width / columnCount;
        float ratio = width / rowRectTransform.rect.width;

        float height = rowRectTransform.rect.height * ratio;
        int rowCount = itemCount / columnCount;
        if (itemCount % rowCount > 0)
            rowCount++;
        //Debug.Log("width:" + width);
        //Debug.Log("height:" + height);
        //Debug.Log("ratio:" + ratio);

        //adjust the height of the container so that it will just barely fit all its children
        float scrollHeight = height * rowCount;
        containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
        containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);

        int j = 0;
        for (int i = 0; i < itemCount; i++)
        {
            //this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
            if (i % columnCount == 0)
                j++;

            //create a new item, name it, and set the parent
            GameObject newItem = Instantiate(itemPrefab) as GameObject;
            newItem.name = gameObject.name + " item at (" + i + "," + j + ")";
            //newItem.transform.parent = gameObject.transform;
            newItem.transform.SetParent(gameObject.transform, false);

            //move and size the new item
            RectTransform rectTransform = newItem.GetComponent<RectTransform>();

            float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
            float y = containerRectTransform.rect.height / 2 - height * j;
            rectTransform.offsetMin = new Vector2(x, y);



            x = rectTransform.offsetMin.x + width;
            y = rectTransform.offsetMin.y + height;
            rectTransform.offsetMax = new Vector2(x, y);
            rectTransform.localScale.Set(1, 1, 1);

            //for (int i = 0; i < 20; i++)
            //{
            //GameObject goButton = (GameObject)Instantiate(prefabButton);
            //goButton.transform.SetParent(ParentPanel, false);
            //goButton.transform.localScale = new Vector3(1, 1, 1);




            //tempButton.GetComponentInChildren<Text>().text = classes[i].courseName;

            //int tempInt = i;
            //string uid = classes[i].cid.ToString();
            ClassInfo c = classes[i];

            //Transform classTr= newItem.transform.Find("ClassName");
            Text className = newItem.transform.Find("ClassName").GetComponent<Text>();
            className.text = c.courseName;

            Text teacherName = newItem.transform.Find("TeacherName").GetComponent<Text>();
            teacherName.text = c.teacherName;

            Text daysTaught = newItem.transform.Find("DaysTaught").GetComponent<Text>();
            daysTaught.text = c.daysTaught;

            Button joinButton = newItem.transform.Find("Join").GetComponent<Button>();
            joinButton.onClick.AddListener(() => JoinClick(c));

            Button deleteButton = newItem.transform.Find("Delete").GetComponent<Button>();

            if (p.privilege != "T")
            {
                //Debug.Log("Disabbling button");
                //deleteButton.GetComponent<Image>().enabled = false;
                //deleteButton.enabled = false;
                deleteButton.gameObject.SetActive(false);
            }
            else
            {
                deleteButton.onClick.AddListener(() => DeleteClick(c));
            }
        }

        //scrollBar.GetComponent<Scrollbar>().value = 1;
    }

    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));

        ExitTitle = ExitTitle.GetComponent<Text>();
        ExitMenu = ExitMenu.GetComponent<Canvas>();
        ExitMenu.enabled = false;
        StartCoroutine(getClassInfo());
    }

    IEnumerator getClassInfo()
    {

        WWWForm form = new WWWForm();
        
        GameObject t = GameObject.Find("Title");
        Text title = t.GetComponent<Text>();

        form.AddField("UID", p.uid);

        WWW download = new WWW(RequestHelper.URL_GET_CLASSES, form);

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
                title.text = "You have no Lectures";
                //Debug.Log("Invalid input");
            }
            else {
                string[] rows = data.Split(';');
                List<ClassInfo> classes = new List<ClassInfo>();

                for (int i = 0; i < rows.Length; ++i)
                {

                    if (rows[i].Trim().Length > 0)
                    {
                        //Debug.Log("Adding " + rows[i]);
                        classes.Add(new ClassInfo(rows[i]));
                    }
                }
                if (p.privilege == "T")
                {
                    title.text = "My Lectures";

                }
                createButtons(classes);
                scrollBar.GetComponent<Scrollbar>().value = 1;
            }
        }
    }

    void JoinClick(ClassInfo c)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        PlayerInfo p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));
        p.setRoomName(c.cid + "");
        //Debug.Log("Button clicked = " + buttonNo);

        //SceneManager.LoadScene("classroom");
        //SceneManager.LoadScene("classroom-48" );
        SceneManager.LoadScene("classroom-" + c.maxEnrolled);
    }

    void DeleteClick(ClassInfo c)
    {

        ExitTitle.text = "Are you sure you want to delete your \"" + c.courseName + "\" course? (can't be undone)";
        cid = c.cid;
        ExitMenu.enabled = true;
        //p.setRoomName(c.courseName);
        //SceneManager.LoadScene("classroom");
    }

    public void YesClick()
    {
        //Debug.Log("UID:" + p.uid + "|CID:" + cid);
        StartCoroutine(deleteClass());
        SceneManager.LoadScene("ClassesMenu");
    }

    IEnumerator deleteClass()
    {

        WWWForm form = new WWWForm();

        form.AddField("UID", p.uid);
        form.AddField("CID", cid);

        WWW download = new WWW(RequestHelper.URL_DELETE_CLASS, form);


        // Wait until the download is done
        yield return download;

        SceneManager.LoadScene("ClassesMenu");

    }


    public void NoClick()
    {
        cid = -1;
        ExitMenu.enabled = false;
    }

}
