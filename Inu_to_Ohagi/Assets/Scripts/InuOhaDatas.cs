using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InuOhaDatas {
	public enum RoundSetting {thlee,five,Rondom,flee};
	public enum TimeSetting{thirty,sixty,flee};

	public static List<string> playerNames = new List<string> ();
	public static List<string> useCards = new List<string>();
	public static RoundSetting roundSetting;
	public static TimeSetting timeSetting;

	public static string ToStringAsDebug(){
		string output = "*InuOhaDatas*\nPlayerNames:";
		foreach (string s in playerNames)
			output += s + " ";
		output += "\nUseCards:";
		foreach (string s in useCards)
			output += s + " ";
		output += "\nRoundSetting:" + roundSetting + "\nTimeSetting:" + timeSetting;

		output += "\nend";
		return output;
	}
		
}
