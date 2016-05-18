
//namespace ExitGames.Photon.DemoPunVoice
//{

using System.Collections;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VoiceTest : MonoBehaviour
{
    //[SerializeField]
    //private Text punState;
    //[SerializeField]
    //private Text voiceState;

    //private Canvas canvas;

    //[SerializeField]
    //private Button punSwitch;
    //private Text punSwitchText;
    //[SerializeField]
    //private Button voiceSwitch;
    //private Text voiceSwitchText;
    //[SerializeField]
    //private Button calibrateButton;
    //private Text calibrateText;

    //[SerializeField]
    //private Text voiceDebugText;

    private PhotonVoiceRecorder rec;

    //[SerializeField]
    //private GameObject inGameSettings;

    //[SerializeField]
    //private GameObject globalSettings;

    //[SerializeField]
    //private Text devicesInfoText;

    //private GameObject debugGO;

    private bool debugMode;

    private bool prevTalk;

    PlayerInfo p;

    public delegate void OnDebugToggle(bool debugMode);

    public static event OnDebugToggle DebugToggled;

    [SerializeField]
    private int calibrationMilliSeconds = 2000;

	//---------------------------------------------------------
	//ADDING VARIABLES FIXME
	string fileName;
	string filePath;


	int BUFFSIZ;
	float[]  buffer;
	float[]  buffer1;
	float[]  buffer2;

	bool sendBuf1 = false;
	bool sendBuf2 = false;

	int frag = 0;
	int fragSize;

	float playtime;
	int count = 0;
	float interval = 1.0F;

	public AudioClip mic;
	public AudioSource source;

	bool notSending;
	//---------------------------------------------------------

    private void OnEnable()
    {
        NetworkManager.CharacterInstantiated += CharacterInstantiation_CharacterInstantiated;
        //ChangePOV.CameraChanged += OnCameraChanged;
        //CharacterInstantiation.CharacterInstantiated += CharacterInstantiation_CharacterInstantiated;
        //BetterToggle.ToggleValueChanged += BetterToggle_ToggleValueChanged;
    }

    private void OnDisable()
    {
        NetworkManager.CharacterInstantiated -= CharacterInstantiation_CharacterInstantiated;
        //ChangePOV.CameraChanged -= OnCameraChanged;
        //CharacterInstantiation.CharacterInstantiated -= CharacterInstantiation_CharacterInstantiated;
        //BetterToggle.ToggleValueChanged -= BetterToggle_ToggleValueChanged;
    }

    private void CharacterInstantiation_CharacterInstantiated(GameObject character)
    {
        rec = character.GetComponent<PhotonVoiceRecorder>();
        rec.enabled = true;
    }

    private void InitToggles(Toggle[] toggles)
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            Toggle toggle = toggles[i];
            switch (toggle.name)
            {
                case "Mute":
                    toggle.isOn = (AudioListener.volume <= 0.001f);
                    break;

                case "AutoTransmit":
                    toggle.isOn = PhotonVoiceSettings.Instance.AutoTransmit;
                    break;

                case "VoiceDetection":
                    toggle.isOn = PhotonVoiceSettings.Instance.VoiceDetection;
                    break;

                case "AutoConnect":
                    toggle.isOn = PhotonVoiceSettings.Instance.AutoConnect;
                    break;

                case "AutoDisconnect":
                    toggle.isOn = PhotonVoiceSettings.Instance.AutoDisconnect;
                    break;

                case "DebugVoice":
                    //DebugMode = PhotonVoiceSettings.Instance.DebugInfo;
                    //toggle.isOn = DebugMode;
                    break;

                case "Transmit":
                    toggle.isOn = (rec != null && rec.Transmit);
                    break;

                case "DebugEcho":
                    toggle.isOn = PhotonVoiceNetwork.Client.DebugEchoMode;
                    break;

                default:
                    break;

            }
        }
    }

    //private void CharacterInstantiation_CharacterInstantiated(GameObject character)
    //{
    //    rec = character.GetComponent<PhotonVoiceRecorder>();
    //    rec.enabled = true;
    //}

    private float volumeBeforeMute;

    private void BetterToggle_ToggleValueChanged(Toggle toggle)
    {
        switch (toggle.name)
        {
            case "Mute":
                //AudioListener.pause = toggle.isOn;
                if (toggle.isOn)
                {
                    volumeBeforeMute = AudioListener.volume;
                    AudioListener.volume = 0f;
                }
                else
                {
                    AudioListener.volume = volumeBeforeMute;
                    volumeBeforeMute = 0f;
                }
                break;
            case "Transmit":
                if (rec)
                {
                    rec.Transmit = toggle.isOn;
                }
                break;
            case "VoiceDetection":
                PhotonVoiceSettings.Instance.VoiceDetection = toggle.isOn;
                if (rec)
                {
                    rec.Detect = toggle.isOn;
                }
                break;
            case "DebugEcho":
                PhotonVoiceNetwork.Client.DebugEchoMode = toggle.isOn;
                break;
            case "AutoConnect":
                PhotonVoiceSettings.Instance.AutoConnect = toggle.isOn;
                break;

            case "AutoDisconnect":
                PhotonVoiceSettings.Instance.AutoDisconnect = toggle.isOn;
                break;
            case "AutoTransmit":
                PhotonVoiceSettings.Instance.AutoTransmit = toggle.isOn;
                break;
            case "DebugVoice":
                //DebugMode = toggle.isOn;
                //PhotonVoiceSettings.Instance.DebugInfo = DebugMode;
                break;

            default:
                break;
        }
    }

    private void OnCameraChanged(Camera newCamera)
    {
        //canvas.worldCamera = newCamera;
    }

    private void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerInfo");
        p = (PlayerInfo)gos[0].GetComponent(typeof(PlayerInfo));

        prevTalk = !p.canTalk;
        volumeBeforeMute = AudioListener.volume;

		//ADDING FIXME
		mic = null;

		fileName = p.uid + "-" + DateTime.Now.ToString("MM-dd");
		filePath = "";

		//MOVED TO WHERE MIC GETS SET
		//BUFFSIZ = 44100;
		//buffer = new float[(int) interval * BUFFSIZ];

		playtime = Time.time + interval;

		notSending = true;
    }

    private void PunSwitchOnClick()
    {
        if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
        {
            PhotonNetwork.Disconnect();
        }
        else if (PhotonNetwork.connectionStateDetailed == PeerState.Disconnected ||
            PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated)
        {
#if UNITY_5_3
            PhotonNetwork.ConnectUsingSettings(string.Format("1.{0}", UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex));
#else
                PhotonNetwork.ConnectUsingSettings(string.Format("1.{0}", Application.loadedLevel));
#endif
        }
    }

    private void VoiceSwitchOnClick()
    {
        if (PhotonVoiceNetwork.ClientState == ExitGames.Client.Photon.LoadBalancing.ClientState.Joined)
        {
            PhotonVoiceNetwork.Disconnect();
        }
        else if (PhotonVoiceNetwork.ClientState == ExitGames.Client.Photon.LoadBalancing.ClientState.Disconnected)
        {
            PhotonVoiceNetwork.Connect();
        }
    }

    private void CalibrateButtonOnClick()
    {
        if (rec && !rec.VoiceDetectorCalibrating)
        {
            rec.VoiceDetectorCalibrate(calibrationMilliSeconds);
        }
    }

    private void Update()
    {
//        // editor only two-ways binding for toggles
//#if UNITY_EDITOR
//        //InitToggles(globalSettings.GetComponentsInChildren<Toggle>());
//#endif

        if (rec != null && p.canTalk != prevTalk)
        {
            rec.Transmit = p.canTalk;
            prevTalk = p.canTalk;
        }
        switch (PhotonNetwork.connectionStateDetailed)
        {
            case PeerState.PeerCreated:
            case PeerState.Disconnected:
                //punSwitch.interactable = true;
                //punSwitchText.text = "PUN Connect";
                if (rec != null)
                {
                    rec.enabled = false;
                    rec = null;
                }
                break;
            case PeerState.Joined:
                //punSwitch.interactable = true;
                //punSwitchText.text = "PUN Disconnect";
                break;
            default:
                //punSwitch.interactable = false;
                //punSwitchText.text = "PUN busy";
                break;
        }
        switch (PhotonVoiceNetwork.ClientState)
        {
            case ExitGames.Client.Photon.LoadBalancing.ClientState.Joined:
                //voiceSwitch.interactable = true;
                //voiceSwitchText.text = "Voice Disconnect";
                //inGameSettings.SetActive(true);
                //InitToggles(inGameSettings.GetComponentsInChildren<Toggle>());
                if (rec != null)
                {
                    //calibrateButton.interactable = !rec.VoiceDetectorCalibrating;
                    //calibrateText.text = rec.VoiceDetectorCalibrating ? "Calibrating" : string.Format("Calibrate ({0}s)", calibrationMilliSeconds / 1000);
                }
                else {
                    //calibrateButton.interactable = false;
                    //calibrateText.text = "Unavailable";
                }
                break;
            case ExitGames.Client.Photon.LoadBalancing.ClientState.Uninitialized:
            case ExitGames.Client.Photon.LoadBalancing.ClientState.Disconnected:
                switch (PhotonNetwork.connectionStateDetailed)
                {
                    case PeerState.Joined:
                        //voiceSwitch.interactable = true;
                        //voiceSwitchText.text = "Voice Connect";
                        //voiceDebugText.text = "";
                        break;
                    default:
                        //voiceSwitch.interactable = false;
                        //voiceSwitchText.text = "Voice N/A";
                        //voiceDebugText.text = "";
                        break;
                }
                //calibrateButton.interactable = false;
                //calibrateText.text = "Unavailable";
                //inGameSettings.SetActive(false);
                break;
            default:
                //voiceSwitch.interactable = false;
                //voiceSwitchText.text = "Voice busy";
                break;
        }
        if (debugMode)
        {
            //punState.text = string.Format("PUN: {0}", PhotonNetwork.connectionStateDetailed);
            //voiceState.text = string.Format("PhotonVoice: {0}", PhotonVoiceNetwork.ClientState);
            if (rec != null && rec.LevelMeter != null)
            {
                //voiceDebugText.text = string.Format("Amp: avg. {0}, peak {1}",
                //        rec.LevelMeter.CurrentAvgAmp.ToString("0.000000"),
                //        rec.LevelMeter.CurrentPeakAmp.ToString("0.000000"));
            }
        }

		//----------------------------------------------------
		//ADDING THINGS
		//-----------------------------------------------------



		if (rec != null && rec.mic != null && p.canTalk && mic != rec.mic.mic) {
			Debug.Log ("I am in VoiceTest");
			mic = rec.mic.mic;
			fragSize = mic.frequency / 10;
			buffer = new float[fragSize];
			buffer1 = new float[fragSize * 100];
			buffer2 = new float[fragSize * 100];
		} else {
			Debug.Log ("I am in VoiceTest and cannot speak");
		}
			
		if (mic && p.privilege == "T") {
			/*
			source = GetComponent<AudioSource> ();
			source.clip = mic;
			source.Play ();
		}
		*/
			if (GetData (buffer) && frag++ < 200) {
				if (frag <= 100) {
					buffer.CopyTo(buffer1, (frag - 1) * fragSize);
				} else {
					buffer.CopyTo(buffer2, (frag - 101) * fragSize);
				}
				if (frag == 100) {
					sendBuf1 = true;
				} else if (frag == 200) {
					Debug.Log ("I am setting sendBuf2 to true!!!");
					sendBuf2 = true;
					frag = 0;
				}
			}
			if ((sendBuf1 || sendBuf2) && notSending) {
				if (filePath != "") {
					UnityEditor.FileUtil.DeleteFileOrDirectory (filePath);

					filePath = "";
				}
				float[] buf = buffer1;
				if (sendBuf2) {
					buf = buffer2;
				}

				Debug.Log (System.DateTime.Today.ToString ());

				string tempfilename = fileName + '-' + count.ToString ();
				notSending = false;

				AudioClip clip = AudioClip.Create ("frag", buf.Length, mic.channels, mic.frequency, false, false);
				clip.SetData (buf, 0);

				if (sendBuf2) {
					Array.Clear(buffer2, 0, fragSize * 100);
					sendBuf2 = false;
				} else {
					Array.Clear(buffer1, 0, fragSize * 100);
					sendBuf1 = false;
				}

				filePath = Save (tempfilename, clip);
				//filePath = Save(tempfilename, mic);
				count++;

				StartCoroutine (Send (filePath, tempfilename));
			} 
			//AudioSource.PlayClipAtPoint (mic, transform.position);

			//first = false;
		}
    }

	//---------------------------------------------------------------
	//ADDING THINGS
	//------------------------------------------------------------

	IEnumerator Send(string filepath, string filename) {
		WWW localfile = new WWW (filepath);
		yield return localfile;

		WWWForm form = new WWWForm ();

		form.AddBinaryData ("file", localfile.bytes, filename);

		WWW w = new WWW ("http://52.38.66.127/scripts/voiceUpload.php", form);
		yield return w;

		notSending = true;
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

	//The following code was not written by us, credit goes to user darktable of github
	//https://gist.github.com/darktable/2317063

	const int HEADER_SIZE = 44;

	public static string Save(string filename, AudioClip clip) {
		if (!filename.ToLower().EndsWith(".wav")) {
			filename += ".wav";
		}

		var filepath = Path.Combine(Application.persistentDataPath, filename);

		Debug.Log(filepath);

		// Make sure directory exists if user is saving to sub dir.
		Directory.CreateDirectory(Path.GetDirectoryName(filepath));

		using (var fileStream = CreateEmpty(filepath)) {

			ConvertAndWrite(fileStream, clip);

			WriteHeader(fileStream, clip);
		}

		return filepath; // TODO: return false if there's a failure saving the file
	}


	static FileStream CreateEmpty(string filepath) {
		var fileStream = new FileStream(filepath, FileMode.Create);
		byte emptyByte = new byte();

		for(int i = 0; i < HEADER_SIZE; i++) //preparing the header
		{
			fileStream.WriteByte(emptyByte);
		}

		return fileStream;
	}

	static void ConvertAndWrite(FileStream fileStream, AudioClip clip) {

		var samples = new float[clip.samples];

		clip.GetData(samples, 0);

		Int16[] intData = new Int16[samples.Length];
		//converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]

		Byte[] bytesData = new Byte[samples.Length * 2];
		//bytesData array is twice the size of
		//dataSource array because a float converted in Int16 is 2 bytes.

		float rescaleFactor = 32767; //to convert float to Int16

		for (int i = 0; i<samples.Length; i++) {
			intData[i] = (short) (samples[i] * rescaleFactor);
			Byte[] byteArr = new Byte[2];
			byteArr = BitConverter.GetBytes(intData[i]);
			byteArr.CopyTo(bytesData, i * 2);
		}

		fileStream.Write(bytesData, 0, bytesData.Length);
	}

	static void WriteHeader(FileStream fileStream, AudioClip clip) {

		var hz = clip.frequency;
		var channels = clip.channels;
		var samples = clip.samples;

		fileStream.Seek(0, SeekOrigin.Begin);

		Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
		fileStream.Write(riff, 0, 4);

		Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
		fileStream.Write(chunkSize, 0, 4);

		Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
		fileStream.Write(wave, 0, 4);

		Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
		fileStream.Write(fmt, 0, 4);

		Byte[] subChunk1 = BitConverter.GetBytes(16);
		fileStream.Write(subChunk1, 0, 4);

		UInt16 two = 2;
		UInt16 one = 1;

		Byte[] audioFormat = BitConverter.GetBytes(one);
		fileStream.Write(audioFormat, 0, 2);

		Byte[] numChannels = BitConverter.GetBytes(channels);
		fileStream.Write(numChannels, 0, 2);

		Byte[] sampleRate = BitConverter.GetBytes(hz);
		fileStream.Write(sampleRate, 0, 4);

		Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
		fileStream.Write(byteRate, 0, 4);

		UInt16 blockAlign = (ushort) (channels * 2);
		fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

		UInt16 bps = 16;
		Byte[] bitsPerSample = BitConverter.GetBytes(bps);
		fileStream.Write(bitsPerSample, 0, 2);

		Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
		fileStream.Write(datastring, 0, 4);

		Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
		fileStream.Write(subChunk2, 0, 4);

		//		fileStream.Close();
	}
	/*

	struct ClipData
	{
		public int samples;
	}
	private System.Threading.Thread WritingThread;

	void ConvertAndWrite(System.IO.MemoryStream memStream, ClipData clipData)
	{
		float[] samples = new float[clipData.samples];

		Int16[] intData = new Int16[samples.Length];

		Byte[] bytesData = new Byte[samples.Length * 2];

		const float rescaleFactor = 32767; //to convert float to Int16

		for (int i = 0; i < samples.Length; i++)
		{
			intData[i] = (short)(samples[i] * rescaleFactor);
		}
		Buffer.BlockCopy(intData, 0, bytesData, 0, bytesData.Length);
		memStream.Write(bytesData, 0, bytesData.Length);
	}

	public string Save(string filename, AudioClip clip)
	{
		if (!filename.ToLower().EndsWith(".wav"))
			filename += ".wav";
		string filepath = System.IO.Path.Combine(Application.persistentDataPath, filename);
		Debug.Log(filepath);
		System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filepath));
		ClipData clipData = new ClipData();
		clipData.samples = clip.samples;
		using (System.IO.FileStream fileStream = CreateEmpty(filepath))
		{
			System.IO.MemoryStream memStream = new System.IO.MemoryStream();
			WritingThread = new System.Threading.Thread(() => ConvertAndWrite(memStream, clipData));
			memStream.WriteTo(fileStream);
			WriteHeader(fileStream, clip);
		}
		return filepath;
	}

	static System.IO.FileStream CreateEmpty(string filepath) {
		var fileStream = new System.IO.FileStream(filepath, FileMode.Create);
		byte emptyByte = new byte();

		for(int i = 0; i < HEADER_SIZE; i++) //preparing the header
		{
			fileStream.WriteByte(emptyByte);
		}

		return fileStream;
	}

	static void WriteHeader(FileStream fileStream, AudioClip clip) {

		var hz = clip.frequency;
		var channels = clip.channels;
		var samples = clip.samples;

		fileStream.Seek(0, SeekOrigin.Begin);

		Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
		fileStream.Write(riff, 0, 4);

		Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
		fileStream.Write(chunkSize, 0, 4);

		Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
		fileStream.Write(wave, 0, 4);

		Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
		fileStream.Write(fmt, 0, 4);

		Byte[] subChunk1 = BitConverter.GetBytes(16);
		fileStream.Write(subChunk1, 0, 4);

		UInt16 two = 2;
		UInt16 one = 1;

		Byte[] audioFormat = BitConverter.GetBytes(one);
		fileStream.Write(audioFormat, 0, 2);

		Byte[] numChannels = BitConverter.GetBytes(channels);
		fileStream.Write(numChannels, 0, 2);

		Byte[] sampleRate = BitConverter.GetBytes(hz);
		fileStream.Write(sampleRate, 0, 4);

		Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
		fileStream.Write(byteRate, 0, 4);

		UInt16 blockAlign = (ushort) (channels * 2);
		fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

		UInt16 bps = 16;
		Byte[] bitsPerSample = BitConverter.GetBytes(bps);
		fileStream.Write(bitsPerSample, 0, 2);

		Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
		fileStream.Write(datastring, 0, 4);

		Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
		fileStream.Write(subChunk2, 0, 4);

		//		fileStream.Close();
	}
	*/

}

//}