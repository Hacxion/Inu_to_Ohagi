using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerSetting : MonoBehaviour {


	public void Initialize(){
		Clear ();
	}

	void Update(){
		playerNumText.text = "" + playerNodeList.Count;
	}

	List<PlayerNode> playerNodeList = new List<PlayerNode>();
	[SerializeField] GameObject nodeObj = null;
	[SerializeField] GameObject content = null;
	[SerializeField] Text playerNumText = null;

	public void GenerateNewNode(){
		GameObject newNode = Instantiate (nodeObj);
		newNode.transform.SetParent (content.transform);
		PlayerNode newPlayerNode = newNode.GetComponent<PlayerNode> ();
		newPlayerNode.Initialize (this,playerNodeList.Count+1);
		playerNodeList.Add (newPlayerNode);
	}

	public void DeleteNode(PlayerNode target){
		int addr = playerNodeList.IndexOf (target);
		playerNodeList.RemoveAt (addr);
		for (int i = addr; i < playerNodeList.Count; i++) {
			playerNodeList [i].Setnum (i + 1);
		}
		Destroy (target.gameObject);
	}

	public void Clear(){
		while (playerNodeList.Count > 0) {
			Destroy (playerNodeList [0].gameObject);
			playerNodeList.RemoveAt (0);
		}
		GenerateNewNode ();
	}

	public List<string> GetPlayerNames(){
		List<string> playerNames = new List<string> ();
		foreach (PlayerNode pn in playerNodeList) {
			playerNames.Add (pn.GetName());
		}
		return playerNames;
	}

}
