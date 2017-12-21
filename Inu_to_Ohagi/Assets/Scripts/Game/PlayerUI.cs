using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PlayerUI : MonoBehaviour {
	List<GamePlayerNode> playerNodeList = new List<GamePlayerNode>();
	[SerializeField] GameObject nodeObj = null;
	[SerializeField] GameObject content = null;

	// Use this for initialization
	void Start () {

	}

	void Update(){
		UpdateDisplay ();
	}

	public void Initialize(List<PlayerData> playerDatas){
		foreach (PlayerData pd in playerDatas) {
			GenerateNewNode (pd);
		}
	}


	public void GenerateNewNode(PlayerData playerData){
		GameObject newNode = Instantiate (nodeObj);
		newNode.transform.SetParent (content.transform);
		GamePlayerNode newPlayerNode = newNode.GetComponent<GamePlayerNode> ();
		newPlayerNode.Initialize(playerData);
		playerNodeList.Add (newPlayerNode);
	}

	public void UpdateDisplay(){
		foreach (GamePlayerNode gpn in playerNodeList) {
			gpn.UpdateDisplay ();
		}
	}

}
