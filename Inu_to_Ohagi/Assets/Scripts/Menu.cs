using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	float timeScale;
	[SerializeField] GameObject menuTitleWindow = null;
	[SerializeField] GameObject menuQuitWindow = null;
	[SerializeField] GameObject menuSettingWindow = null;

	// Use this for initialization
	void Start () {
		timeScale = Time.timeScale;
		Time.timeScale = 0;
	
	}
	
	public void Deth(){
		Time.timeScale = timeScale;
		Destroy (this.gameObject);
	}

	public void PushSettingButton(){
		GameObject obj =  Instantiate (menuSettingWindow);
		obj.transform.SetParent (GameObject.Find ("Canvas").transform,false);
	}

	public void PushTitleButton(){
		GameObject obj =  Instantiate (menuTitleWindow);
		obj.transform.SetParent (GameObject.Find ("Canvas").transform,false);
	}

	public void PushQuitButton(){
		GameObject obj =  Instantiate (menuQuitWindow);
		obj.transform.SetParent (GameObject.Find ("Canvas").transform,false);
	}

	public void PushBackButton(){
		Deth ();
	}
}
