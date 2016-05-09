using UnityEngine;
using System.Collections;

public class ClassInfo  {

    public int cid;
    public string courseName;
    public string startDate;
    public string endDate;
    public string daysTaught;
    public int maxEnrolled;
    public string teacherName;


    public ClassInfo(string row)
    {
        cid = int.Parse(RequestHelper.GetValue(row, "CID"));
        courseName = RequestHelper.GetValue(row, "courseName");
        startDate= RequestHelper.GetValue(row, "startDate");
        endDate = RequestHelper.GetValue(row, "endDate");
        daysTaught = RequestHelper.GetValue(row, "daysTaught");
        //maxEnrolled = int.Parse(RequestHelper.GetValue(row, "maxEnrolled"));
        teacherName = RequestHelper.GetValue(row, "firstName") + " " + RequestHelper.GetValue(row, "lastName");
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
