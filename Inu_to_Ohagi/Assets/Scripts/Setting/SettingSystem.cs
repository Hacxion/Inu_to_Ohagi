using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class SettingSystem : MonoBehaviour {
	[SerializeField] Text playerNumText = null;
	[SerializeField] PlayerSetting playerSetting = null;
	[SerializeField] Text useCardText = null;
	[SerializeField] CardSetting cardSetting = null;
	[SerializeField] Button startButton = null;
	[SerializeField] ChoiceUI roundChoice = null;
	[SerializeField] ChoiceUI timeChoice = null;


	void Start(){
		playerSetting.Initialize ();
		UpdatePlayerNumText ();
		cardSetting.Initialize ();
		UpdateUseCardText ();
		if (!InuOhaDatas.firstPlay) {
			roundChoice.Choice ((int)InuOhaDatas.roundSetting);
		}
	}

	void Update(){
		if (Input.GetKey("escape"))
			Application.Quit();
	}

	public void UpdatePlayerNumText(){
		playerNumText.text = "" + playerSetting.GetPlayerNames().Count + " 人";
	}

	public void UpdateUseCardText(){
		List<string> useCardTitles = cardSetting.GetUseCardTitles ();
		if (useCardTitles.Count == 0) {
			useCardText.text = "カードが設定されていません";
			startButton.interactable = false;
		}
		else {
			string connectedTitle = useCardTitles [0];
			for (int i = 1; i < useCardTitles.Count; i++) {
				connectedTitle += (" + " + useCardTitles [i]);
			}
			useCardText.text = connectedTitle;
			startButton.interactable = true;
		}
	}

	public void PushStartButton(){
		InuOhaDatas.firstPlay = false;
		InuOhaDatas.playerNames = playerSetting.GetPlayerNames ();
		InuOhaDatas.isDefaultNames = playerSetting.GetIsDefaultNames ();
		InuOhaDatas.useCards = cardSetting.GetUseCardDatas ();
		InuOhaDatas.roundSetting = (InuOhaDatas.RoundSetting ) roundChoice.getChoice ();

		SceneManager.LoadScene("Game");
	}
	

}
