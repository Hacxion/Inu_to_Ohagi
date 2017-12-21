using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {
	[SerializeField]GameObject menueObj = null;


	public void Push(){
		GameObject obj =  Instantiate (menueObj);
		obj.transform.SetParent (GameObject.Find ("Canvas").transform,false);

	}
}
