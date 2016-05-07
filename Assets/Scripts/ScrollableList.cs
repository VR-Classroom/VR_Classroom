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

            Button tempButton = newItem.GetComponent<Button>();
            tempButton.GetComponentInChildren<Text>().text = classes[i].courseName;

            //int tempInt = i;
            string uid = classes[i].cid.ToString();


            tempButton.onClick.AddListener(() => ButtonClicked(uid));
        }

        //scrollBar.GetComponent<Scrollbar>().value = 1;
    }

    void Start()
    {

        StartCoroutine(waitCheck());
    }

    IEnumerator waitCheck()
    {

        WWWForm form = new WWWForm();

        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        PlayerInfo p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));


        GameObject t = GameObject.Find("Title");
        Text title = t.GetComponent<Text>();

        form.AddField("UID", p.uid);

        WWW download = new WWW(RequestHelper.URL_GET_CLASSES,form);
        

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
                title.text = "You are not teaching any classes";
                //Debug.Log("Invalid input");
            }
            else {
                string[] rows = data.Split(';');
                List<ClassInfo> classes = new List<ClassInfo>();
                
                for(int i =0; i < rows.Length; ++i)
                {

                    if (rows[i].Trim().Length > 0)
                    {
                        //Debug.Log("Adding " + rows[i]);
                        classes.Add(new ClassInfo(rows[i]));
                    }
                }
                if(p.privilege == "T")
                {

                    //if(classes.Count == 0 || rows.Length == 0)
                    //{
                    //    title.text = "You are not teaching any classes";
                    //}
                    //else
                    //{
                        title.text = "Classes you are teaching";
                    //}

                    //test.text = "I'm a teacher";

                    //PlayerInfo p = (PlayerInfo)t.GetComponent(typeof(PlayerInfo));
                }
                createButtons(classes);
                scrollBar.GetComponent<Scrollbar>().value = 1;
                //Debug.Log(GetValue(rows[0], "email"));
                //Debug.Log(download.text);
                //GameObject t = GameObject.Find("PlayerInfo");
                //PlayerInfo p = (PlayerInfo)t.GetComponent(typeof(PlayerInfo));
                //p.initPlayer(data);
                //Debug.Log("Valid User. TODO: save data before moving to new scene");

                //SceneManager.LoadScene("classroom");
                //SceneManager.LoadScene("ClassesMenu");
            }
        }
    }

    void ButtonClicked(string courseName)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        PlayerInfo p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));
        p.setRoomName(courseName);
        //Debug.Log("Button clicked = " + buttonNo);

        SceneManager.LoadScene("classroom");
    }

}
