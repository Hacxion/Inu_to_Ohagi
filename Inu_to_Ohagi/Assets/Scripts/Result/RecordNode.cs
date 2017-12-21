using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class RecordNode : MonoBehaviour {
	[SerializeField] Text runkText = null;
	[SerializeField] Text nameText = null;
	[SerializeField] Text pointText = null;

	public void Initialize(Record record,float height){
		runkText.text = record.GetRunk () + "位";
		nameText.text = record.GetName ();
		pointText.text = record.GetPoint () + "Pt";
		gameObject.GetComponent<LayoutElement> ().minHeight = height;
	}
}
