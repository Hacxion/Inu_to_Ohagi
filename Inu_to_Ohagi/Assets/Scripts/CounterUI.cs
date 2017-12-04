using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CounterUI : MonoBehaviour {

	[SerializeField] int firstNum=0;
	[SerializeField] int min=0;
	[SerializeField] int max=0;
	private int num;
	[SerializeField] Text numberText=null;
	[SerializeField] Button downButton=null;
	[SerializeField] Button upButton=null;
	private int preNum;

	// Use this for initialization
	void Start () {
		Reset ();
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

	public void Reset(){
		num = firstNum;
		preNum = firstNum;
		CounterUpdate ();
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
