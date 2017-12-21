using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InuOhaDatas:MonoBehaviour {
	public enum RoundSetting {thlee,five,Rondom,flee};
	public static bool firstPlay = true;
	public static List<string> playerNames = new List<string> ();
	public static List<bool> isDefaultNames = new List<bool> ();
	public static List<CardData> useCards = new List<CardData>();
	public static RoundSetting roundSetting;
	public static List<Record> records = new List<Record> ();
	public static float bgmVolume = -20;
	public static float seVolume = -20;

	public static string ToStringAsDebug(){
		string output = "*InuOhaDatas*\nPlayerNames:";
		foreach (string s in playerNames)
			output += s + " ";
		output += "\nUseCards:";
		foreach (CardData cd  in useCards)
			output += cd.GetTitle() + " ";
		output += "\nRoundSetting:" + roundSetting ;

		output += "\nend";
		return output;
	}

	public static string GetOutSideDataFolderPass(){
		//winならアプリケーション名Data/
		if (Application.platform == RuntimePlatform.WindowsPlayer)
			return Application.dataPath;
		//macならapp位置
		else if (Application.platform == RuntimePlatform.OSXPlayer)
			return Application.dataPath + "/../../";
		//エディタ内ならAassets内
		else
			return Application.dataPath;
	}
		
}
