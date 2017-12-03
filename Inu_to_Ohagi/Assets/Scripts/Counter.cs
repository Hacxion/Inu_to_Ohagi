using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CounterUI : MonoBehaviour {

	[SerializeField] int firstNum;
	[SerializeField] int min;
	[SerializeField] int max;
	private int num;
	[SerializeField] Text numberText;
	[SerializeField] Button downButton;
	[SerializeField] Button upButton;
	private int preNum;

	// Use this for initialization
	void Start () {
		num = firstNum;
		preNum = firstNum;
		CounterUpdate ();
	}

	void Update(){
		if (preNum != num)
			CounterUpdate ();
	}

	public void Up(){
		num++;
	}

	public void Down(){
		num--;
	}

	public int GetNum(){
		return num;
	}

	public void SetNum(int num){
		this.num = num;
	}

	public void ReSetNum(){
		this.num = firstNum;
	}

	public void CounterUpdate(){
		preNum = num;
		numberText.text = "" + num;
		if (num >= max)
			upButton.interactable = false;
		else
			upButton.interactable = true;
		if (num <= min)
			downButton.interactable = false;
		else
			downButton.interactable = true;
	}
		
}
