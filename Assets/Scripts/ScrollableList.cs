using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollableList : MonoBehaviour
{
    public GameObject itemPrefab;
    public int itemCount = 10, columnCount = 1;
    public Scrollbar scrollBar;

    void Start()
    {
        RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform>();
        RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();

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

            int tempInt = i;

            tempButton.onClick.AddListener(() => ButtonClicked(tempInt));
            //}

            //RectTransform.
        }

        scrollBar.GetComponent<Scrollbar>().value = 1;


    }

    void ButtonClicked(int buttonNo)
    {
        Debug.Log("Button clicked = " + buttonNo);
        SceneManager.LoadScene("classroom");
    }

}
