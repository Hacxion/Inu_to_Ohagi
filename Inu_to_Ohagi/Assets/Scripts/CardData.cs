using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CardData : MonoBehaviour {
	List<string> pinchs = new List<string> ();
	List<string> items = new List<string>();

	void Start(){
		List<string> cardTitles = InuOhaDatas.useCards;
		foreach (string s in cardTitles) {
			AddCardsFromTitle (s);
		}

		DebugContents ();
	}


	public bool AddCardsFromTitle(string title){
		StreamReader sr = new StreamReader (Application.dataPath + "/OutsideData/" + title + ".txt");
		string line;

		/*-Pinch-探し*/
		for(;;){
			if(sr.EndOfStream == true) return false;
			line = sr.ReadLine();
			if (line == "-Pinch-") break;
		}
		/*-Item-が見つかるまでピンチを格納*/
		for(;;){
			if(sr.EndOfStream == true) return false;
			line = sr.ReadLine();
			if (line == string.Empty) continue;
			if(line == "-Item-") break;
			pinchs.Add(line);
		}

		/*残りの行をアイテムに格納*/
		while (sr.EndOfStream != true) {
			line = sr.ReadLine ();
			if (line == string.Empty)
				continue;
			items.Add (line);
		}
		return true;

	}

	public void DebugContents(){
		string debugLine = "Pinchs;";
		foreach (string s in pinchs) {
			debugLine += s + " ";
		}
		debugLine += "\nItems:";
		foreach (string s in items) {
			debugLine += s + " ";
		}
		Debug.Log (debugLine);
	}


}
