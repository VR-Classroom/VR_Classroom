using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterMenu : MonoBehaviour
{

    //public Button Register;
    //public Button MainMenu;


    public InputField firstName;
    public InputField lastName;
    public InputField email;
    public InputField password;
    public InputField password2;
    public Dropdown gender;
    public Dropdown studentTeacher;
    public Dropdown userTitle;


    private string fname;
    private string lname;
    private string emailTxt;
    private string pass;
    private string pass2;
    private string gen;
    private string priv;
    private string title;



    //private string loginurl = "http://52.38.66.127/scripts/registerUser.php";

    private bool validLogin = false;
    private float checkRate = 1.0f;
    //private MyWebRequest mwr = new MyWebRequest();

    private Text invalidText;

    void Update()
    {

        if (email.isFocused == true)
        {
            email.GetComponent<Image>().color = Color.white;
            invalidText.enabled = false;
        }
        else if (password.isFocused == true)
        {
            password.GetComponent<Image>().color = Color.white;
            invalidText.enabled = false;
        }
        else if (password2.isFocused == true)
        {
            password2.GetComponent<Image>().color = Color.white;
            invalidText.enabled = false;
        }
        else if (firstName.isFocused == true)
        {
            firstName.GetComponent<Image>().color = Color.white;
            invalidText.enabled = false;
        }
        else if (lastName.isFocused == true)
        {
            lastName.GetComponent<Image>().color = Color.white;
            invalidText.enabled = false;
        }
    }

    // Use this for initialization
    void Start()
    {
        //Register = Register.GetComponent<Button>();
        //MainMenu = MainMenu.GetComponent<Button>();

        firstName = firstName.GetComponent<InputField>();
        lastName = lastName.GetComponent<InputField>();
        email = email.GetComponent<InputField>();
        password = password.GetComponent<InputField>();
        password2 = password2.GetComponent<InputField>();

        gender = gender.GetComponent<Dropdown>();
        studentTeacher = studentTeacher.GetComponent<Dropdown>();
        userTitle = userTitle.GetComponent<Dropdown>();

        invalidText = GameObject.Find("Invalid").GetComponent<Text>();
        invalidText.enabled = false;

    }

    public void MainMenuPress()
    {
        SceneManager.LoadScene("LoginMenu");
    }

    private void showInvalid(string text)
    {
        invalidText.text = text;
        invalidText.enabled = true;
    }

    public void RegisterPress()
    {
        //Debug.Log("gender:" + gender.value);
        //Debug.Log("gender:"+gender.options[gender.value].text);
        //Debug.Log("St:" + studentTeacher.value);
        //Debug.Log("St:" + studentTeacher.options[studentTeacher.value].text);

        fname = firstName.text.Trim();
        lname = lastName.text.Trim();
        emailTxt = email.text.Trim();
        pass = password.text.Trim();
        pass2 = password2.text.Trim();
        gen = gender.options[gender.value].text.Trim();
        priv = studentTeacher.options[studentTeacher.value].text.Trim();
        title = userTitle.options[userTitle.value].text.Trim();

        bool valid = true;

        if (string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname)
            || string.IsNullOrEmpty(emailTxt) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(pass2)
            || string.IsNullOrEmpty(gen) || string.IsNullOrEmpty(priv) || string.IsNullOrEmpty(title))
        {
            Debug.Log("A field was empty");
            showInvalid("All fields are required!");
            valid = false;
            //return;
        }

        if (valid && pass != pass2)
        {
            password.GetComponent<Image>().color = Color.red;
            password2.GetComponent<Image>().color = Color.red;
            showInvalid("Passwords do not match");
            Debug.Log("Passwords do not match");
            valid = false;
            //return;
        }
        if (valid && !emailTxt.Contains("@"))
        {
            email.GetComponent<Image>().color = Color.red;
            showInvalid("Invalid email");
            Debug.Log("Invalid email");
            valid = false;
            //return;
        }



        if (valid)
        {
            if (title == "Title")
            {
                title = "";
            }

            if (gen == "Specify Gender")
            {
                Debug.Log("Gender not specified");
                gen = "O";
            }
            else if (gen == "Male")
            {
                gen = "M";
            }
            else if (gen == "Female")
            {
                gen = "F";
            }
            else if (gen == "Other")
            {
                gen = "O";
            }


            if (priv == "Student")
                priv = "S";
            else if (priv == "Teacher")
                priv = "T";
            else
                Debug.Log("Priv:" + priv);
            StartCoroutine(waitCheck());
        }


    }



    IEnumerator waitCheck()
    {


        Debug.Log("Creating form!");
        WWWForm form = new WWWForm();
        //form.AddField("EMAIL", "hgarc014@ucr.edu");
        //    form.AddField("PASS", "1234");



        form.AddField("FNAME", fname);
        form.AddField("LNAME", lname);
        form.AddField("GENDER", gen);
        form.AddField("PRIV", priv);
        //form.AddField("BIRTH", birth);
        form.AddField("EMAIL", emailTxt);
        form.AddField("PASS", pass);
        form.AddField("TITLE", title);

        WWW download = new WWW(RequestHelper.URL_REGISTER, form);

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
                if(data.Contains("Duplicate entry") && data.Contains("email"))
                {
                    email.GetComponent<Image>().color = Color.red;
                    showInvalid("that email already exists");
                }
                else if (data.Contains("ERROR"))
                {
                    showInvalid("Oops. An Error Occured!");
                }
                else
                    SceneManager.LoadScene("LoginMenu");
            }
        }

    }

}
