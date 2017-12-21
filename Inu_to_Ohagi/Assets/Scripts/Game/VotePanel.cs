using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VotePanel : MonoBehaviour {
	[SerializeField] GameObject nodeObj = null;
	[SerializeField] GameObject content = null;
	[SerializeField] Text voteNumtext = null;
	List<VoteNode> voteNodeList = new List<VoteNode>();

	void Update(){
		voteNumtext.text = "投票数:" + GetChoicedNum () + "/" + voteNodeList.Count;
	}


	void Start(){
	}

	public void Initialize(List<string> names){
		while (voteNodeList.Count > 0) {
			Destroy (voteNodeList [0].gameObject);
			voteNodeList.RemoveAt (0);
		}
		foreach (string s in names) {
			GenerateNewNode (s);
		}
		if (names.Count == 0) {
			GenerateNewNode ("");
		}
	}

	void GenerateNewNode(string name){
		GameObject newNode = Instantiate (nodeObj);
		newNode.transform.SetParent (content.transform);
		VoteNode newVoteNode = newNode.GetComponent<VoteNode> ();
		newVoteNode.Initialize(name);
		voteNodeList.Add (newVoteNode);
	}

	public void Clean(){
		foreach(VoteNode vn in voteNodeList){
			vn.Clean();
		}
	}

	public int GetVoterNum(){
		return voteNodeList.Count;
	}

	public int GetChoicedNum(){
		int count = 0;
		foreach(VoteNode vn in voteNodeList){
			if (vn.GetIsChoiced ())
				count++;
		}
		return count;
	}

	public bool GetIsOk(){
		int count = 0;
		foreach(VoteNode vn in voteNodeList){
			if (vn.GetIsOk ())
				count++;
		}

		//半数以上であれば合格
		if (count >= (voteNodeList.Count + 1) / 2)
			return true;
		else
			return false;
		
	}
}
