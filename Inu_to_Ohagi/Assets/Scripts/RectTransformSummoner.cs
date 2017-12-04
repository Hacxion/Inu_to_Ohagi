using UnityEngine;
using System.Collections;

public class RectTransformSummoner : MonoBehaviour {
	[SerializeField] RectTransform target = null;
	[SerializeField] Vector3 goal = Vector3.zero;
	private Vector3 originPoint;

	// Use this for initialization
	void Start () {
		originPoint = target.position;
	
	}

	public void Summon(){
		target.position = goal;
	}

	public void Back(){
		target.position = originPoint;
	}
		
}
