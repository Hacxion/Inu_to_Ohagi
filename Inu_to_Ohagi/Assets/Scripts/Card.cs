using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour {
	[SerializeField] Text text = null;
	RectTransform rect;
	Vector3 start,goal;
	float moveTime,remainTime;
	AudioSource slideAudio,putAudio;
	float soundDelay = -1;
	public enum SoundType{Slide,Put};
	SoundType soundType;

	void Awake(){
		
	}

	public void Initialize (string content){
		text.text = content;
	}

	// Use this for initialization
	void Start () {
		remainTime = 0f;
		AudioSource[] sources;
		sources = GetComponents<AudioSource> ();
		slideAudio = sources [0];
		putAudio = sources [1];
		rect = this.GetComponent<RectTransform> ();
	}

	// Update is called once per frame
	void Update () {
		Moving ();
		Sounding ();
	}

	/*
	 * カードをgoalにtimeかけて動かす。初期位置や終了お知らせポインタを指定可能
	 */
	public void Move(Vector3 goal, float time){
		start = rect.position;
		this.goal = goal;
		this.moveTime = remainTime =  time;
	}

	public void Move(Vector3 start,Vector3 goal, float time){
		rect.position = start;
		Move (goal, time);
	}




	void Moving(){
		
		if (remainTime > 0f) {
			remainTime -= Time.deltaTime;
			if (remainTime <= 0) {
				remainTime = 0f;
				rect.position = goal;
				return;
			} else {
				float ratio = (moveTime - remainTime) / moveTime;
				Vector3 nextVec = new Vector3 ((goal.x - start.x) * ratio + start.x, (goal.y - start.y) * ratio + start.y, 0f);
				rect.position = nextVec;
			}
		}
	}

	public void SetSound(SoundType soundType,float delay){
		soundDelay = delay;
		this.soundType = soundType;
	}

	public void Sounding(){
		if (soundDelay >= 0) {
			soundDelay -= Time.deltaTime;
			if (soundDelay < 0) {
				if (soundType == SoundType.Slide)
					slideAudio.PlayOneShot (slideAudio.clip);
				else if (soundType == SoundType.Put)
					putAudio.PlayOneShot (putAudio.clip);
				soundDelay = -1;
			}
		}
	}

	public bool GetisMoving(){
		if (remainTime > 0)
			return true;
		else
			return false;
	}

	public string GetText(){
		return text.text;
	}

	public void SetText(string text){
		this.text.text = text;
	}




}
