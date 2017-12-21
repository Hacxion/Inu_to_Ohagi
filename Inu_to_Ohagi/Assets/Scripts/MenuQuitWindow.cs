using UnityEngine;
using System.Collections;

public class MenuQuitWindow : MonoBehaviour {
	public void PushOk(){
		Application.Quit ();
	}

	public void PushNo(){
		Destroy (this.gameObject);
	}
}
