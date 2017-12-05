using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardNode : MonoBehaviour {
	[SerializeField] Toggle toggle = null;
	[SerializeField] Text label = null;

	public void Initialize(string title){
		toggle.isOn = false;
		label.text = title;
	}
	public void SetIsOn(bool isOn){
		toggle.isOn = isOn;
	}

	public string GetTitle(){
		return label.text;
	}

	public bool GetIsOn(){
		return toggle.isOn;
	}
}
