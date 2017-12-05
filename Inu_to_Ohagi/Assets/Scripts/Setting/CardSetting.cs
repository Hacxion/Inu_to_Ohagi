using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CardSetting : MonoBehaviour {
	List<CardNode> cardNodeList = new List<CardNode>();
	[SerializeField] GameObject nodeObj = null;
	[SerializeField] GameObject content = null;

	// Use this for initialization
	void Start () {

	}

	public void Initialize(){
		DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/OutsideData");
		FileInfo[] info = dir.GetFiles("*.txt");
		for(int i=0;i<info.Length;i++){
			GenerateNewNode (Path.GetFileNameWithoutExtension(info [i].Name));
		}
		cardNodeList [0].SetIsOn (true);
	}


	public void GenerateNewNode(string title){
		GameObject newNode = Instantiate (nodeObj);
		newNode.transform.SetParent (content.transform);
		CardNode newCardNode = newNode.GetComponent<CardNode> ();
		newCardNode.Initialize(title);
		cardNodeList.Add (newCardNode);
	}

	public List<string> GetUseCardTitles(){
		List<string> useCardTitles = new List<string> ();
		foreach (CardNode cn in cardNodeList) {
			if (cn.GetIsOn() == true)
				useCardTitles.Add (cn.GetTitle ());
		}
		return useCardTitles;
	}

}
