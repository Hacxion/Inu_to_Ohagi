using UnityEngine;
using System.Collections;

public class PlayerData{
	string playerName;
	int point;
	string[] items = new string[3] ;
	bool[] isUsed = new bool[3] ;

	public void Initialize(string name){
		this.playerName = name;
		point = 0;
		for(int i=0;i<3;i++){
			items[i] = "？";
			isUsed[i] = true;
		}
	}



	/*
	 * ゲッタセッタ達
	 */
	public string GetName(){
		return playerName;
	}

	public int GetPoint(){
		return point;
	}
	public void SetPoint(int point){
		this.point = point;
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

}
