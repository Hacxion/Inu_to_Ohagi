using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemZone : MonoBehaviour {
	[SerializeField] Card[] itemCards = new Card[3];
	PlayerData playerData;
	bool isChoiceable;
	bool[] isChoiced = new bool[3];
	[SerializeField] float moveLength = 30f;

	void Start(){
		Initialize ();
	}

	public void Initialize(){
		for (int i = 0; i < 3; i++){
			isChoiced [i] = false; 
		}

		SetIsChoiceable (false);
	}

	public void ClickCard(int index){
		//既に選ばれてたなら負に
		if (isChoiced [index]) {
			isChoiced [index] = false;
			//RecttransFormを取得して位置をずらす処理
			RectTransform rec = itemCards [index].GetComponent<RectTransform> ();
			Vector3 vec = rec.position;
			vec.y -= moveLength;
			rec.position = vec;

		}
		//選ばれてなければ正に
		else {
			isChoiced [index] = true;
			//RecttransFormを取得して位置をずらす処理
			RectTransform rec = itemCards [index].GetComponent<RectTransform> ();
			Vector3 vec = rec.position;
			vec.y += moveLength;
			rec.position = vec;
		}

		itemCards [index].SetSound (Card.SoundType.Put, 0f);
	}

	public void SetIsChoiceable(bool isChoiceable){
		for (int i = 0; i < 3; i++) {
			itemCards [i].GetComponent<Button> ().interactable = isChoiceable;
		}
	}

	public int GetChoiceCount(){
		int count = 0;
		for(int i=0;i<3;i++){
			if(isChoiced[i]) count ++;
		}
		return count;
	}

	public bool[] GetChoiceStatus(){
		return isChoiced;
	}



}
