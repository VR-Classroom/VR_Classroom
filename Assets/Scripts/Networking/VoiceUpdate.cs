using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class VoiceUpdate : MonoBehaviour {


    public bool canTalk;

    private PhotonVoiceRecorder rec;

    private Toggle toggle;

    private bool prev;

    // Use this for initialization
    void Start () {
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
}
