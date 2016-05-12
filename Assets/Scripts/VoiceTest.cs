
//namespace ExitGames.Photon.DemoPunVoice
//{

using System.Collections;
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
    }
}

//}