using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugMessageWindw : MonoBehaviour {
	string message;
	void Awake(){
		this.name = "DebugMessageWindow";
	}

	void  Update(){
		this.transform.SetAsLastSibling ();
	}

	public void AddMessage(string message){
		this.message += "\n" + message;
		this.transform.FindChild ("Message").GetComponent<Text> ().text = this.message;
	}


	public static void SetMessage(string message){
		GameObject obj = GameObject.Find ("DebugMessageWindow");
		if (obj == null) {
			obj = Instantiate ((GameObject)Resources.Load ("Prefabs/DebugMessageWindow"));
			obj.transform.SetParent (GameObject.Find ("Canvas").transform,false);
		}
		obj.GetComponent<DebugMessageWindw> ().AddMessage (message);
	}
}
