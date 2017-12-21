using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardNode : MonoBehaviour {
	[SerializeField] Toggle toggle = null;
	[SerializeField] Text label = null;
	CardData cardData = new CardData();

	public void Initialize(CardData cardData){
		this.cardData = cardData;
		toggle.isOn = false;
		label.text = cardData.GetTitle();
	}
	public void SetIsOn(bool isOn){
		toggle.isOn = isOn;
	}

	public string GetTitle(){
		return cardData.GetTitle();
	}

	public bool GetIsOn(){
		return toggle.isOn;
	}

	public CardData GetCardData(){
		return cardData;
	}
}
