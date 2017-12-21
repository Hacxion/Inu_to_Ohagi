using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class RecordSystem : MonoBehaviour {
	List<Record> records = new List<Record>();
	[SerializeField] RecordPanel recordPanel = null;

	// Use this for initialization
	void Start () {
		records = InuOhaDatas.records;

		/*ここサンプルデータ挿入所*/
		/*
		SetSampleLecord("ハクシ1", 0);
		SetSampleLecord ("ハクシ2", 3);
		SetSampleLecord ("ハクシ3", 2);
		SetSampleLecord ("ハクシ4", 2);
		*/

		Record.SortRecords (records);
		DebugRecords ();

		recordPanel.Initialize (records);
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("escape"))
			Application.Quit();
	}

	void SetSampleLecord(string name,int point){
		Record sampleRecord = new Record (name);
		for (int i = 0; i < point; i++) {
			sampleRecord.AddIsOk (true);
		}
		records.Add (sampleRecord);
	}

	void DebugRecords(){
		foreach (Record rc in records) {
			Debug.Log (rc.GetRunk () + "位:" + rc.GetName () + "---" + rc.GetPoint ());
		}
	}

	public void PushToTitleButton(){
		SceneManager.LoadScene ("Title");
	}

}
