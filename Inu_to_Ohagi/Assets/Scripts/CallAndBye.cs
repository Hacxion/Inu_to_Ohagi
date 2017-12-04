using UnityEngine;
using System.Collections;

public class CallAndBye : MonoBehaviour {
	[SerializeField] Vector3 goal = Vector3.zero;
	private Vector3 home;
	private RectTransform ownRect;

	void Start(){
		ownRect = this.GetComponent<RectTransform> ();
		home = ownRect.position;
	}

	public void Call(){
		ownRect.position = goal;
	}

	public void Bye(){
		ownRect.position = home;
	}
}
