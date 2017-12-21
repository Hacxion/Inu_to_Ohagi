using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerNode: MonoBehaviour {
	bool isDefault;
	PlayerSetting playerSetting;
	[SerializeField] Text numText = null;
	[SerializeField] InputField inputField = null;
	[SerializeField] Text placeholder = null;
	[SerializeField] Button deleteButton = null;

	void Update(){
		if (playerSetting.GetPlayerNames ().Count <= 1)
			deleteButton.interactable = false;
		else
			deleteButton.interactable = true;
	}
	/*
	 * デフォルトとして初期化
	 */
	public void Initialize(PlayerSetting playerSetting,int num){
		this.playerSetting = playerSetting;
		numText.text = "" + (num);
		inputField.text = "";
		placeholder.text = "プレイヤー" + (num);
	}

	/*
	 * 名前を設定して初期化
	 */
	public void Initialize(PlayerSetting playerSetting,int num,string name){
		this.playerSetting = playerSetting;
		numText.text = "" + (num);
		inputField.text = name;
		Debug.Log (name + "/" + inputField.text);
		placeholder.text = "プレイヤー" + (num);
	}


	public void Setnum(int num){
		numText.text = "" + (num);
		placeholder.text = "プレイヤー" + (num);
	}

	public void PushDeleteButton(){
		playerSetting.DeleteNode (this);
	}

	public string GetName(){
		if (inputField.text == string.Empty)
			return placeholder.text;
		else
			return 
				inputField.text;
	}

	public bool GetIsDefaultName(){
		if (inputField.text == string.Empty)
			return true;
		else
			return false;
	}
}
