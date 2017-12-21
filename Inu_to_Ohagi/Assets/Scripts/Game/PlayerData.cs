using UnityEngine;
using System.Collections;

public class PlayerData{
	string playerName;
	string[] items = new string[3] ;
	bool[] isUsed = new bool[3] ;
	Record record;

	public void Initialize(string name){
		this.playerName = name;
		for(int i=0;i<3;i++){
			items[i] = "？";
			isUsed[i] = true;
		}
		record = new Record (playerName);
	}



	/*
	 * ゲッタセッタ達
	 */
	public string GetName(){
		return playerName;
	}

	public int GetPoint(){
		return record.GetPoint ();
	}

	public void AddIsOk(bool isOk){
		record.AddIsOk (isOk);
	}

	public string[] GetItems(){
		return items;
	}
	public void SetItems(string[] items){
		this.items = items;
	}

	public bool[] GetIsUsed(){
		return isUsed;
	}
	public void SetIsUsed(bool[] isUsed){
		this.isUsed = isUsed;
	}

	public Record GetRecord(){
		return record;
	}

}
