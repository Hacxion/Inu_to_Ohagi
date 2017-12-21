using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Record {
	string name;
	int point;
	List<bool> isOks = new List<bool>();
	int runk;

	public Record(string name){
		this.name = name;
		point = 0;
		isOks.Clear ();
	}

	public void SetRunk(int runk){
		this.runk = runk;
	}


	public static void CheckRunk(List<Record> records){
		int checkedNum = 0;
		int tempRunk = 1;
		bool[] checkFlag = new bool[records.Count];
		for (int i = 0; i < checkFlag.Length; i++) {
			checkFlag [i] = false;
		}

		//全ての記録にランクをつけるまでループ
		while (checkedNum < records.Count) {
			int maxPoint = -1;
			//現段階最大ポイントをチェック
			for(int i=0;i<records.Count;i++){
				if (checkFlag [i])
					continue;
				int tempPoint = records[i].GetPoint ();
				if (tempPoint > maxPoint) {
					maxPoint = tempPoint;
				}
			}
			//最大ポイントと同じポイントを持つ記録に順位を代入
			for(int i=0;i<records.Count;i++){
				if (checkFlag [i])
					continue;
				if (records[i].GetPoint () == maxPoint) {
					records[i].SetRunk (tempRunk);
					checkFlag [i] = true;
					checkedNum++;
				}
			}
			tempRunk++;
		}

	}

	public static int CompareRunk(Record xRecord,Record yRecord){
		return xRecord.GetRunk () - yRecord.GetRunk ();
	}
		
	public static void SortRecords(List<Record> records){
		CheckRunk (records);
		records.Sort (CompareRunk);
	}


	public int GetPoint(){
		return point;
	}

	public void AddIsOk(bool isOk){
		isOks.Add (isOk);
		if (isOk) {
			point++;
		}
	}

	public int GetRunk(){
		return runk;
	}

	public string GetName(){
		return name;
	}
}

/*
public class RecordComparer:IComparer{
	public int Compare(object x,object y){
		Record xRecord = (Record)x;
		Record yRecord = (Record)y;
		return xRecord.GetRunk () - yRecord.GetRunk ();
	}
}
*/