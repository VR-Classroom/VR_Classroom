using UnityEngine;
//using System.IO;
//using System.Text;
//using UnityEngine.Microphone;
using System.Collections;

public class Mic : MonoBehaviour {

	int lastSample = 0;
	public AudioSource audio;
	AudioClip c;
	string fileName = "";
	bool Recording = true;
	public float timestamp;
	public float interval = 2.0F;

	void Start() {
		audio = GetComponent<AudioSource> ();

		//var teacher = new WWW (); //check if there is already someone recording

		//Recording = second; //if you are the teacher than you should record

		if (Recording) {
			c = Microphone.Start ("Microphone", true, 10, 44100);
			while (!(Microphone.GetPosition (null) > 0)) {}
		}

		timestamp = Time.time;

		/*

		float[] sample = new float[1];
		sample [0] = 0.5F;

		byte[] data = ToByte(sample);
		fileName = "V";

		WWWForm form = new WWWForm ();
		form.AddBinaryData ("file", data, fileName);

		WWW w = new WWW ("http://52.38.66.127/scripts/voiceUpload.php", form);
		*/

		/*
		var w = new WWW ("http://52.38.66.127/voiceData/sample.mp3");

		while (w.progress < 0.08) {
			//yield WaitForSeconds (0.1);
		}

		var a = w.GetAudioClip(false, true, AudioType.MPEG);
		audio.clip = a;
		audio.Play ();
		*/
	}


	void Update () {
		if (timestamp < Time.time) {
			if (!Recording) {
				//if you are not recording you should download audio and queue it
				;
			} else {
				float[] sample = new float[1];
				sample [0] = 0.5F;

				byte[] data = ToByte(sample);
				fileName = "V";

				WWWForm form = new WWWForm ();
				form.AddBinaryData ("file", data, fileName);

				WWW w = new WWW ("http://52.38.66.127/scripts/voiceUpload.php", form);
				/*
				int pos = Microphone.GetPosition (null);
				int diff = pos - lastSample;

				if (diff > 0) {
					float[] samples = new float[diff * audio.clip.channels];
					audio.clip.GetData (samples, lastSample);
					byte[] data = ToByte (samples);
					//Send (data);

					fileName = timestamp.ToString();

					WWWForm form = new WWWForm ();

					//form.AddField ("action", "voice upload");

					form.AddBinaryData ("file", data, fileName);

					WWW w = new WWW ("http://52.38.66.127/scripts/voiceUpload.php", form);

					//WWW w = new WWW ("http://52.38.66.127/scripts/voiceUpload.php");

					lastSample = pos;
				}
				*/
			}
			timestamp += interval;
		}
	}

	IEnumerator Send(byte[] data) {
		fileName = "V";

		WWWForm form = new WWWForm ();
		//form.AddField ("voice", data);

		form.AddBinaryData ("file", data, fileName);

		WWW w = new WWW ("http://52.38.66.127/scripts/voiceUpload.php", form);

		yield return w;
	/*
		if (w.error != null) {
			Debug.Log(w.error);//
		} else {
			yield return new WaitForSeconds (5);

			WWW w2 = new WWW ("http://52.38.66.127/voiceData" + fileName);
			audio = GetComponent<AudioSource> ();
			audio.clip = w2.audioClip;
			audio.Play ();
		}
		*/
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

	public float[] ToFloat(byte[] data) {
		float[] returnArray = new float[data.Length / 4];
		for (int i = 0; i < returnArray.Length; i += 4) {
			returnArray[i] = System.BitConverter.ToSingle(data, i);
		}

		return returnArray;
	}
}