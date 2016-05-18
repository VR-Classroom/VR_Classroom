using UnityEngine;
using System.Collections;

public class PlayerInfo : Photon.MonoBehaviour
{

    public string fname;
    public string lname;
    public string gender;
    public string privilege;
    public string email;
    public string uid;
    public string dispName;
    public string title;
    public string roomJoin;
    public int classSize;

    public bool canTalk;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(uid);
            stream.SendNext(canTalk);
            stream.SendNext(dispName);
        }
        else {
            uid = (string) stream.ReceiveNext();
            canTalk= (bool)stream.ReceiveNext();
            dispName = (string)stream.ReceiveNext();
        }
    }

    public void setRoomName(string roomName)
    {
        roomJoin = roomName;
    }

    public void initPlayer(string uid, string fname, string lname, string gender, string privilege, string email,string title)
    {
        this.fname = fname;
        this.lname = lname;
        this.gender = gender;
        this.privilege = privilege;
        this.email = email;
        this.uid = uid;
        this.title = title;
        
        if (string.IsNullOrEmpty(title) || privilege.Equals("S") )//|| title.Equals(" "))
        {
            this.dispName = fname;
        }
        else
        {
            this.dispName = title + " " + lname;
        }
        
    } 

    public void initPlayer(string mysqlData)
    {

        string[] rows = mysqlData.Split(';');
        string row = rows[0];
        string[] fields = { "UID", "firstName", "lastName", "gender", "privilege", "email","title" };
        string[] fieldValues = new string[fields.Length];
        for (int i = 0; i < fields.Length; ++i)
        {
            
            fieldValues[i] = RequestHelper.GetValue(row, fields[i]);
        }

        initPlayer(fieldValues[0], fieldValues[1], fieldValues[2], fieldValues[3], fieldValues[4], fieldValues[5],fieldValues[6]);
    }

    public void resetInfo()
    {
        fname = null;
        lname = null;
        gender = null;
        privilege = null;
        email = null;
        uid = null;
        dispName = null;
    }

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }


    // Update is called once per frame
    void Update()
    {

    }

    
}
