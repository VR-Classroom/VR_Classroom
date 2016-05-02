using UnityEngine;
//using System.IO;
//using System.Text;
//using UnityEngine.Microphone;
using System.Collections;

public class Mic : MonoBehaviour {

	int lastSample = 0;
	public AudioSource audio;
	string fileName = "";
	bool Recording = false;

	void Start() {
		audio = GetComponent<AudioSource> ();
		var w = new WWW ("http://52.38.66.127/voiceData/sample.mp3");

		while (w.progress < 0.08) {
			//yield WaitForSeconds (0.1);
		}

		var a = w.GetAudioClip(false, true, AudioType.MPEG);
		audio.clip = a;
		audio.Play ();
		//var audio = GetComponent<AudioSource> ();
		//audio.clip = Microphone.Start ("Microphone", true, 10, 44100);
		//audio.loop = true;
		//while (!(Microphone.GetPosition (null) > 0)) {}
		//audio.Play ();
	}

	/*void Update () {
		if (!Recording) {
			audio.clip = Microphone.Start ("Microphone", true, 10, 44100);
			Recording = true;
		} else {
			int pos = Microphone.GetPosition (null);
			int diff = pos - lastSample;

			if (diff > 0) {
				float[] samples = new float[diff * audio.clip.channels];
				audio.clip.GetData (samples, lastSample);
				byte[] data = ToByte (samples);
				//Send (data);

				fileName = "V" + (Time.time).ToString ();

				WWWForm form = new WWWForm ();

				form.AddField ("action", "voice upload");
				form.AddField ("name", fileName);
				//form.AddField ("voice", data);

				form.AddBinaryData ("file", data, fileName, "");

				WWW w = new WWW ("http://52.38.66.127/scripts/voiceUpload.php", form);

				//WWW w = new WWW ("http://52.38.66.127/scripts/voiceUpload.php");
			}
			lastSample = pos;
		}
	}*/

	IEnumerator Send(byte[] data) {
		fileName = "V" + (Time.time).ToString ();

		WWWForm form = new WWWForm ();

		form.AddField ("action", "voice upload");
		form.AddField ("name", fileName);
		//form.AddField ("voice", data);

		form.AddBinaryData ("file", data, fileName, "");

		WWW w = new WWW ("http://52.38.66.127/scripts/voiceUpload.php", form);

		yield return w;
		if (w.error != null) {
			Debug.Log(w.error);//
		} else {
			yield return new WaitForSeconds (5);

			WWW w2 = new WWW ("http://52.38.66.127/voiceData" + fileName);
			audio = GetComponent<AudioSource> ();
			audio.clip = w2.audioClip;
			audio.Play ();
		}
	}

	public byte[] ToByte(float[] samples) {
		byte[] returnArray = new byte[samples.Length * 4];

		int i = 0;
		foreach (float f in samples) {
			byte[] sample = System.BitConverter.GetBytes(f);
			System.Array.Copy(sample, 0, returnArray, i, 4);
			i += 4;
		}

		return returnArray;
	}
}