using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChoiceUI : MonoBehaviour {
	[SerializeField] Button[] buttons = null;
	[SerializeField] int firstChoice = 0;
	private int choice;

	// Use this for initialization
	void Start () {
		Reset ();
	}

	public void Choice(int num){
		buttons[choice].interactable = true;
		buttons [num].interactable = false;
		choice = num;
	}


	public int getChoice(){
		return choice;
	}
		
	public void Reset(){
		choice = firstChoice;
		for (int i = 0; i < buttons.Length; i++) {
			if (i == choice)
				buttons [i].interactable = false;
			else
				buttons [i].interactable = true;
		}
	}
		
}
