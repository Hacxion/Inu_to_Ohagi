using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamePlayerNode : MonoBehaviour {

	PlayerData playerData;
	[SerializeField] Text nameText = null;
	[SerializeField] Text pointText = null;
	[SerializeField] Card[] itemCards = new Card[3];

	public void Initialize(PlayerData playerData){
		this.playerData = playerData;
		UpdateDisplay ();
	}

	public void UpdateDisplay(){
		nameText.text = playerData.GetName ();
		pointText.text = "" + playerData.GetPoint () + "Pt";
		for (int i = 0; i < 3; i++) {
			itemCards [i].SetText (playerData.GetItems () [i]);
		}
	}
}
