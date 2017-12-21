using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SoundSlider : MonoBehaviour {
	public enum AudioType{Master,Bgm,Se};
	[SerializeField] AudioMixer mixer = null;
	[SerializeField] AudioType type = AudioType.Master;
	[SerializeField] GameObject muteIcon = null;
	Slider slider;



	void Start(){
		slider = gameObject.GetComponent<Slider> ();
		float mixerVolume;
		if (type == AudioType.Bgm) {
			mixer.GetFloat ("BgmVolume", out mixerVolume);
			slider.value = mixerVolume;
		} else if (type == AudioType.Se) {
			mixer.GetFloat ("SeVolume", out mixerVolume);
			slider.value = mixerVolume;
		}

		ChangeVolume ();
	}

	public void ChangeVolume(){
		string volumeName;
		if (type == AudioType.Bgm) {
			volumeName = "BgmVolume";
		} else if (type == AudioType.Se) {
			volumeName = "SeVolume";
		} else {
			volumeName = "";
		}
		if (slider.value == slider.minValue) {
			mixer.SetFloat (volumeName, -80.0f);
			muteIcon.SetActive (true);
		} else {
			mixer.SetFloat (volumeName, this.GetComponent<Slider> ().value);
			muteIcon.SetActive (false);
		}

		
	}
}
