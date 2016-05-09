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

	public float uploadTimestamp;
	public float interval = 2.0F;

	public int audioFragmentNum = 0;
	public int mostRecentFragment = 0;
	public int mostRecentUpload = 0;
	int playIndex = 0;
	int channels = 0;

	AudioClip[] fragments;

	void Start() {
		audio = GetComponent<AudioSource> ();
		fragments = new AudioClip[20];

		//var teacher = new WWW (); //check if there is already someone recording

		//Recording = second; //if you are the teacher than you should record

		if (Recording) {
			c = Microphone.Start ("Microphone", true, 10, 44100);
			while (!(Microphone.GetPosition (null) > 0)) {}
		}

		/*
		uploadTimestamp = Time.time;

		var w = new WWW ("http://52.38.66.127/voiceData/zzz2");

		while (!w.isDone) {
			//yield WaitForSeconds (0.1);
		}

		float[] audioArray = ToFloat (w.bytes);
		AudioClip clip = AudioClip.Create("frag", audioArray.Length, 1, 44100, false, false);
		AudioSource.PlayClipAtPoint (clip, new Vector3(100, 100, 0), 1.0f);
		*/
	}

	void Update () {
		if (uploadTimestamp < Time.time) {
			if (!Recording) {
				//if you are not recording you should download audio and queue it
				;
			} else {
				int pos = Microphone.GetPosition (null);
				int diff = pos - lastSample;

				if (diff > 0) {
					float[] samples = new float[diff * c.channels];
					c.GetData (samples, lastSample);
					byte[] data = ToByte (samples);

					fileName = uploadTimestamp.ToString ();

					StartCoroutine (Send (data));

					samples = ToFloat (data);
					AudioClip clip = AudioClip.Create ("frag", data.Length, 1, 44100, false);
					clip.SetData (samples, 0);
					audio.clip = clip;
					audio.Play ();

					lastSample = pos;
				}
			}
			uploadTimestamp += interval;
		} /* else if (!Recording) {
			if (!audio.isPlaying && fragments [playIndex] != null) {
				audio.clip = fragments[playIndex];
				audio.Play ();
			}
			WWW mostRecentUploadReq = new WWW("http://52.38.66.127/scripts/getMostRecent.php");
			var mostRecentUpload = int.Parse (mostRecentUploadReq.text);

			if(mostRecentFragment < mostRecentUpload) {
				if (mostRecentUpload - mostRecentFragment > 20) {
					mostRecentFragment = mostRecentUpload - 15;
				}

				for (int i = mostRecentFragment; i < mostRecentUpload; i++) {
					WWW fragment = new WWW ("http://52.38.66.127/voiceData/zzz" + i.ToString());
					float[] audioArray = ToFloat (fragment.bytes);
					fragments[i] = AudioClip.Create("frag", audioArray.Length, 1, 44100, false, false);
					fragments [i].SetData (audioArray, 0);
				}
			}
		}
		*/
	}

	IEnumerator Send(byte[] data) {
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
		for (int i = 0; i < data.Length; i += 4) {
			returnArray[i/4] = System.BitConverter.ToSingle(data, i);
		}

		return returnArray;
	}
}