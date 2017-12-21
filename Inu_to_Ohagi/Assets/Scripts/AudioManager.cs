using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : Singleton<AudioManager> {
	public AudioMixer mixer;
	enum FadeType{In,Out};
	FadeType fadeTyoe;
	float fadeTime;
	float timeSum = -1;


	void Update(){
		if(timeSum >= 0){
			timeSum += Time.deltaTime;
			if (fadeTyoe == FadeType.In) {
				if (timeSum >= fadeTime) {
					mixer.SetFloat ("MasterVolume", 0);
					timeSum = -1;
				} else {
					mixer.SetFloat ("MasterVolume", Mathf.Lerp (-40, 0, timeSum / fadeTime));
				}
			} else {
				if (timeSum >= fadeTime) {
					mixer.SetFloat ("MasterVolume", -80);
					timeSum = -1;
				} else {
					mixer.SetFloat ("MasterVolume", Mathf.Lerp (0, -40, timeSum / fadeTime));
				}
			}
		}
	}

	public void FadeOut(float time){
		fadeTime = time;
		timeSum = 0;
		fadeTyoe = FadeType.Out;
	}

	public void FadeIn(float time){
		fadeTime = time;
		timeSum = 0;
		fadeTyoe = FadeType.In;
	}


}
