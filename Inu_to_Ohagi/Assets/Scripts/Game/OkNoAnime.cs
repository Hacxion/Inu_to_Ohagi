using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class OkNoAnime : MonoBehaviour {
	[SerializeField] Sprite okGraphic = null;
	[SerializeField] Sprite noGraphic = null;
	bool isOk;
	RectTransform rect;
	Image image;
	float animeTime,remainTime;
	AudioSource okAudio,noAudio;
	bool soundFlag;


	// Use this for initialization
	void Start () {
		rect = this.GetComponent<RectTransform> ();
		image = this.GetComponent<Image> ();
		AudioSource[] souces = GetComponents<AudioSource> ();
		okAudio = souces [0];
		noAudio = souces [1];
	}
	
	// Update is called once per frame
	void Update () {
		Anime ();
			
	
	}


	public void Set(bool isOk,float time){
		this.isOk = isOk;
		if (isOk)
			image.sprite = okGraphic;
		else
			image.sprite = noGraphic;
		rect.localScale = new Vector3 (2.0f, 2.0f, 1.0f);
		animeTime = remainTime = time;
		soundFlag = true;
	}

	void Anime(){
		if (remainTime > 0f) {
			remainTime -= Time.deltaTime;
			if (remainTime <= 0) {
				rect.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
				image.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
				return;
			}
			float bairitsu = 1.0f + Mathf.Pow(remainTime / animeTime,1);
			float alfa = 1.0f - Mathf.Pow(remainTime / animeTime,1);
			rect.localScale = new Vector3 (bairitsu, bairitsu, 1.0f);
			image.color = new Color (1.0f, 1.0f, 1.0f, alfa);
			if (soundFlag && remainTime < 0.8f) {
				soundFlag = false;
				if (isOk)
					okAudio.PlayOneShot (okAudio.clip);
				else
					noAudio.PlayOneShot (noAudio.clip);
			}
		}
	}
}
