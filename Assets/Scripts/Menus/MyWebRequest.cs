using UnityEngine;
using System.Collections;

public class MyWebRequest : MonoBehaviour
{
    private static MyWebRequest m_Instance = null;
    public static MyWebRequest Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (MyWebRequest)FindObjectOfType(typeof(MyWebRequest));
                if (m_Instance == null)
                    m_Instance = (new GameObject("WebRequest")).AddComponent<MyWebRequest>();
                DontDestroyOnLoad(m_Instance.gameObject);
            }
            return m_Instance;
        }
    }
    public static Coroutine Get(string aURL, System.Action<bool, string> aCallback)
    {
        return Instance.StartCoroutine(_GetRequest(aURL, aCallback));
    }
    public static Coroutine Post(string aURL, WWWForm aForm, System.Action<bool, string> aCallback)
    {
        return Instance.StartCoroutine(_PostRequest(aURL, aForm, aCallback));
    }

    private static IEnumerator _GetRequest(string aURL, System.Action<bool, string> aCallback)
    {
        WWW request = new WWW(aURL);
        yield return request;
        if (string.IsNullOrEmpty(request.error))
        {
            if (aCallback != null)
                aCallback(true, request.text);
        }
        else
        {
            if (aCallback != null)
                aCallback(false, request.error);
        }
    }
    private static IEnumerator _PostRequest(string aURL, WWWForm aForm, System.Action<bool, string> aCallback)
    {
        WWW request = new WWW(aURL, aForm);
        yield return request;
        if (string.IsNullOrEmpty(request.error))
        {
            if (aCallback != null)
                aCallback(true, request.text);
        }
        else
        {
            if (aCallback != null)
                aCallback(false, request.error);
        }
    }
}