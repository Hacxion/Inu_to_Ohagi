using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OutPutInuOhaDatas : MonoBehaviour {
	Text text;
	void Start(){
		text = this.GetComponent<Text> ();
	}

	void Update () {
		text.text = InuOhaDatas.ToStringAsDebug();

	}
}
