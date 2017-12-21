using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VoteNode : MonoBehaviour {
	[SerializeField] Toggle okToggle = null;
	[SerializeField] Toggle noToggle = null;
	[SerializeField] Text NameText = null;


	public void Initialize(string name){
		NameText.text = name;
		Clean ();
	}

	public void Clean(){
		okToggle.isOn = false;
		noToggle.isOn = false;
	}

	public bool GetIsChoiced(){
		if(okToggle.isOn || noToggle.isOn)
			return true;
		else 
			return false;
	}

	public bool GetIsOk(){
		if (okToggle.isOn)
			return true;
		else
			return false;
	}
}
