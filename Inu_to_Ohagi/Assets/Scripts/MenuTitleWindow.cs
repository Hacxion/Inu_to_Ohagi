using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuTitleWindow : MonoBehaviour {
	public void PushOk(){
		SceneManager.LoadScene ("Title");
	}

	public void PushNo(){
		Destroy (this.gameObject);
	}
}
