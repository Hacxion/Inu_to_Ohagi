using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CardData {
	string title;
	string fileName;
	List<string> pinchs = new List<string> ();
	List<string> items = new List<string>();


	public bool LoadFromFile(string fileName){
		this.fileName = fileName;
		string filePass = InuOhaDatas.GetOutSideDataFolderPass() + "/CardDatas/" + fileName;
		StreamReader sr = new StreamReader (filePass);
		string line;
		/*-Title-探し*/
		for(;;){
			if(sr.EndOfStream == true) return false;
			line = sr.ReadLine();
			//Debug.Log ("1:" + line);
			if (line == "-Title-") break;
		}

		/*次の文を探してタイトルに設定*/
		for(;;){
			if(sr.EndOfStream == true) return false;
			line = sr.ReadLine();
			//Debug.Log ("2:" + line);
			if (line == string.Empty) continue;
			title = line;

			break;
		}
		/*-Pinch-探し*/
		for(;;){
			if(sr.EndOfStream == true) return false;
			line = sr.ReadLine();
			//Debug.Log ("3:" + line);
			if (line == "-Pinch-") break;
		}
		/*-Item-が見つかるまでピンチを格納*/
		for(;;){
			if(sr.EndOfStream == true) return false;
			line = sr.ReadLine();
			//Debug.Log ("4:" + line);
			if (line == string.Empty) continue;
			if(line == "-Item-") break;
			pinchs.Add(line);
		}

		/*残りの行をアイテムに格納*/
		while (sr.EndOfStream != true) {
			line = sr.ReadLine ();
			//Debug.Log ("5:" + line);
			if (line == string.Empty)
				continue;
			items.Add (line);
		}
		return true;

	}



	public string GetTitle(){
		return title;
	}

	public string GetFileName(){
		return fileName;
	}

	public List<string> GetPinchs(){
		return pinchs;
	}

	public  List<string> GetItems(){
		return items;
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
