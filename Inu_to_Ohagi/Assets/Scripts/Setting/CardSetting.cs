﻿using UnityEngine;
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
		DirectoryInfo dir = new DirectoryInfo(InuOhaDatas.GetOutSideDataFolderPass() +  "/CardDatas");
		FileInfo[] info = dir.GetFiles("*.txt");
		if (info.Length == 0) {
			return;
		}
		for(int i=0;i<info.Length;i++){
			CardData cardData = new CardData ();
			if (cardData.LoadFromFile (info[i].Name)) {
				GenerateNewNode (cardData);
			}
		}
		if (InuOhaDatas.firstPlay)
			cardNodeList [0].SetIsOn (true);
		else {
			foreach (CardData cd in InuOhaDatas.useCards) {
				foreach (CardNode cn in cardNodeList) {
					if (cd.GetFileName () == cn.GetCardData ().GetFileName ())
						cn.SetIsOn (true);
				}
			}
		}
	}


	public void GenerateNewNode(CardData cardData){
		GameObject newNode = Instantiate (nodeObj);
		newNode.transform.SetParent (content.transform);
		CardNode newCardNode = newNode.GetComponent<CardNode> ();
		newCardNode.Initialize(cardData);
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

	public List<CardData> GetUseCardDatas(){
		List<CardData> useCardDatas = new List<CardData> ();
		foreach (CardNode cn in cardNodeList) {
			if (cn.GetIsOn() == true)
				useCardDatas.Add (cn.GetCardData ());
		}
		return useCardDatas;
	}



}
