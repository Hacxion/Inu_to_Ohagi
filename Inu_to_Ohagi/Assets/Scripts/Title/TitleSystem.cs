using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleSystem : Singleton<TitleSystem> {

	// Use this for initialization
	void Start () {
	
	}

	void Update(){
		if (Input.GetKey("escape"))
			Application.Quit();
	}

	public void StartGame(){
		SceneManager.LoadScene("Setting");
	}

	public void FinishGame(){
		Application.Quit ();
	}

}
