
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ExitGames.Photon.DemoPunVoice
{

    [RequireComponent(typeof(Canvas))]
    public class UserHighlight : MonoBehaviour
    {
        private Canvas canvas;


        [SerializeField]
        private PhotonVoiceSpeaker speaker;


        [SerializeField]
        private Image speakerSprite;


        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            if (canvas != null && canvas.worldCamera == null) { canvas.worldCamera = Camera.main; }
        }


        // Update is called once per frame
        private void Update()
        {
            speakerSprite.enabled = speaker != null && speaker.IsPlaying &&
                    PhotonVoiceNetwork.ClientState == ExitGames.Client.Photon.LoadBalancing.ClientState.Joined;
        }

        private void LateUpdate()
        {
            if (canvas == null || canvas.worldCamera == null) { return; } // should not happen, throw error
            transform.rotation = Quaternion.Euler(0f, canvas.worldCamera.transform.eulerAngles.y, 0f); //canvas.worldCamera.transform.rotation;
        }
    }
}
