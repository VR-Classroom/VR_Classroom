using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componentsToDisable;
    GameObject SceenCamera;
    GameObject CardboardMain;

    // Use this for initialization
    void Start () {
        SceenCamera = GameObject.Find("SceenCamera");
        CardboardMain = GameObject.Find("User(Clone)/Sphere/CardboardHead");
        if (!isLocalPlayer)
        {
            Debug.Log(!isLocalPlayer);
            for (int i = 0; i < componentsToDisable.Length; ++i)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        if(SceenCamera != null && SceenCamera.activeSelf)
        {
            if (SceenCamera != null)
            {
                Debug.Log("SceenCamera not null");
                SceenCamera.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("SceenCamera null");
            }
        }
	}

    void OnDisable()
    {
        if (SceenCamera != null)
        {
            Debug.Log("SceenCamera not null");
            SceenCamera.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("SceenCamera null");
        }
    }
}
