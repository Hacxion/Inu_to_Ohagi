using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecordPanel : MonoBehaviour {
	[SerializeField] GameObject nodeObj = null;
	[SerializeField] GameObject content = null;
	[SerializeField] float maxHeight = 120f;
	[SerializeField] float minHeight = 40f;
	int worst = 0;


	public void Initialize(List<Record> records){
		Debug.Log (records.Count);
		worst = records [records.Count - 1].GetRunk();
		foreach (Record rec in records) {
			Debug.Log (rec.GetName () + ":" + rec.GetPoint ());
			GenerateNewNode (rec);
		}
	}
	public void GenerateNewNode(Record record){
		GameObject newNode = Instantiate (nodeObj);
		newNode.transform.SetParent (content.transform);
		RecordNode newrecordNode = newNode.GetComponent<RecordNode> ();
		newrecordNode.Initialize(record,CalcHeight(record.GetRunk()));
	}

	public float CalcHeight(int runk){
		//ワーストが１位　＝　プレイヤーが一人だけのときは問答無用でMaxHeight
		if(worst == 1)
			return maxHeight;
		float delta = (maxHeight - minHeight) / (worst - 1);
		return maxHeight - delta * (runk - 1);
	}


}
