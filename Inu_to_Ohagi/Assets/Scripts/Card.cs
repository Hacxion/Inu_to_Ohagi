using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour {
	[SerializeField] Text text = null;
	RectTransform rect;
	Vector3 start,goal;
	float moveTime,remainTime;
	bool nowIsMoving = false;
	bool preIsMoving = false;

	void Awake(){
		
	}

	public void Initialize (string content){
		text.text = content;
	}

	// Use this for initialization
	void Start () {
		remainTime = 0f;

		rect = this.GetComponent<RectTransform> ();
	}

	// Update is called once per frame
	void Update () {
		
		Moving ();
		preIsMoving = nowIsMoving;
		nowIsMoving = GetisMoving ();
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
			}
			float ratio = (moveTime - remainTime) / moveTime;
			Vector3 nextVec = new Vector3 ((goal.x - start.x) * ratio + start.x, (goal.y - start.y) * ratio + start.y, 0f);
			rect.position = nextVec;
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

	/*
	 * 以前の呼び出しから止まったかどうか
	 */
	public bool CheckStop(){
		if(preIsMoving && !nowIsMoving){
			return true;
		}
		return false;
	}

}
