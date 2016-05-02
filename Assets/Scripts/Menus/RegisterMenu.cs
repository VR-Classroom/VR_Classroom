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


    private string fname;
    private string lname;
    private string emailTxt;
    private string pass;
    private string pass2;
    private string gen;
    private string priv;



    private string loginurl = "http://52.38.66.127/scripts/registerUser.php";

    private bool validLogin = false;
    private float checkRate = 1.0f;
    //private MyWebRequest mwr = new MyWebRequest();


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

    }

    public void MainMenuPress()
    {
        SceneManager.LoadScene("LoginMenu");
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

        bool valid = true;

        if (string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname)
            || string.IsNullOrEmpty(emailTxt) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(pass2)
            || string.IsNullOrEmpty(gen) || string.IsNullOrEmpty(priv))
        {
            Debug.Log("A field was empty");
            valid = false;
            return;
        }

        if (pass != pass2)
        {
            Debug.Log("Passwords do not match");
            valid = false;
            return;
        }
        if (!emailTxt.Contains("@"))
        {
            Debug.Log("Invalid email");
            valid = false;
            return;
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

        if (valid)
        {
            //Debug.Log("Would have created user");
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
                //Debug.Log("Valid User. TODO: save data before moving to new scene");
                SceneManager.LoadScene("LoginMenu");
            }
        }

    }

}
