using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    string highscore_url = "http://52.38.66.127/scripts/getUser.php";
    string playName = "Player 1";
    int score = -1;

    // Use this for initialization
    IEnumerator Start()
    {


        // Create a form object for sending high score data to the server
        WWWForm form = new WWWForm();
        form.AddField("EMAIL", "hgarc014@ucr.edu");
        form.AddField("PASS", "1234");
        
        WWW download = new WWW(highscore_url, form);

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
                string[] rows = data.Split(';');
                Debug.Log(GetValue(rows[0], "email"));
                Debug.Log(download.text);
            }
        }
    }

    string GetValue(string row, string name)
    {
        string value = row.Substring(row.IndexOf(name+":") + name.Length+1);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }
}


