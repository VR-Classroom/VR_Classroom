using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class VoiceUpdate : MonoBehaviour {


    public bool canTalk;

    private PhotonVoiceRecorder rec;

    private Toggle toggle;

    private bool prev;

	int BUFFSIZ;
	float[]  buffer1;
	float[]  buffer2;

	//FIXME GET THE AUDIOCLIP OF THE RECORDING
	public AudioClip mic;

    // Use this for initialization
    void Start () {
		BUFFSIZ = 256;
		float[]  buffer1 = new float[BUFFSIZ];
		float[]  buffer2 = new float[BUFFSIZ];

		mic = rec.rec.mic;
	}


    //private void Start()
    //{
        
    //    toggle = GetComponent<Toggle>();
    //    toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(); });
    //}

    //public void OnToggleValueChanged()
    //{
    //    if (ToggleValueChanged != null)
    //    {
    //        ToggleValueChanged(toggle);
    //    }
    //}


    private void OnEnable()
    {
        //ChangePOV.CameraChanged += OnCameraChanged;
        //NetworkManager.CharacterInstantiated += CharacterInstantiation_CharacterInstantiated;
        //BetterToggle.ToggleValueChanged += BetterToggle_ToggleValueChanged;
    }

    private void CharacterInstantiation_CharacterInstantiated(GameObject character)
    {
        rec = character.GetComponent<PhotonVoiceRecorder>();
        
        rec.enabled = true;
    }

    // Update is called once per frame
    void Update () {
		if (rec && prev != canTalk) {
			Debug.Log ("Got rec.Transmit=" + canTalk);
			rec.Transmit = canTalk;
			prev = canTalk;
		} 
			
    }

	private int micPrevPos;
	private int micLoopCnt;
	private int readAbsPos;

	public bool GetData(float[] buffer)
	{
		int micPos = Microphone.GetPosition(null);
		// loop detection
		if (micPos < micPrevPos)
		{
			micLoopCnt++;            
		}
		micPrevPos = micPos;

		var micAbsPos = micLoopCnt * mic.samples + micPos;

		var bufferSamplesCount = buffer.Length / mic.channels;

		var nextReadPos = readAbsPos + bufferSamplesCount;
		if (nextReadPos < micAbsPos)
		{
			mic.GetData(buffer, readAbsPos % mic.samples);
			readAbsPos = nextReadPos;
			return true;
		}
		else
		{
			return false;
		}        
	}
}
