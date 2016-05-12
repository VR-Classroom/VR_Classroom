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
    public string roomJoin;

    public bool canTalk;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(uid);
            stream.SendNext(canTalk);
            stream.SendNext(fname);
        }
        else {
            uid = (string) stream.ReceiveNext();
            canTalk= (bool)stream.ReceiveNext();
            fname = (string)stream.ReceiveNext();
        }
    }

    public void setRoomName(string roomName)
    {
        roomJoin = roomName;
    }

    public void initPlayer(string uid, string fname, string lname, string gender, string privilege, string email)
    {
        this.fname = fname;
        this.lname = lname;
        this.gender = gender;
        this.privilege = privilege;
        this.email = email;
        this.uid = uid;
    }

    public void initPlayer(string mysqlData)
    {

        string[] rows = mysqlData.Split(';');
        string row = rows[0];
        string[] fields = { "UID", "firstName", "lastName", "gender", "privilege", "email" };
        string[] fieldValues = new string[fields.Length];
        for (int i = 0; i < fields.Length; ++i)
        {
            
            fieldValues[i] = RequestHelper.GetValue(row, fields[i]);
        }

        initPlayer(fieldValues[0], fieldValues[1], fieldValues[2], fieldValues[3], fieldValues[4], fieldValues[5]);
    }

    public void resetInfo()
    {
        fname = null;
        lname = null;
        gender = null;
        privilege = null;
        email = null;
        uid = null;
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
