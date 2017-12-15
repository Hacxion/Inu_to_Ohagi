using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameSystem : MonoBehaviour {
	[SerializeField] Text systemText = null;
	[SerializeField] Text roundText = null;
	[SerializeField] Text playerText = null;
	[SerializeField] Card pinchCard = null;
	[SerializeField] Card numCard = null;
	Vector3 pinchCardPoint,numCardPoint;
	[SerializeField] Card[] itemCards = new Card[3];
	Vector3[] itemCardPints = new Vector3[3];

	public enum Step{standby,think,answer,vote,end};
	Step step;
	float timesum;

	int maxRound,round;
	int order;

	List<string> playerNames,useCards;
	InuOhaDatas.RoundSetting roundSetting;
	List<PlayerData> playerDatas;
	List<string> allPinchs,pinchDeck,pinchGrave;
	List<string> allItems,itemDeck,itemGrave;

	/*ピンチカードとナンバーカードの内容*/
	string pinch;
	int useNum;


	/* デバッグ用初期データ入力群*/
	public bool isUseDebugData = false;
	public List<string> dd_players;
	public List<string> dd_titles;
	public InuOhaDatas.RoundSetting dd_round;

	// Use this for initialization
	void Start () {
		
		if(isUseDebugData) InputInuOhaData ();
		LoadInuOhaData ();
		Initialize ();
		StandbyHead ();
		timesum = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("escape"))
			Application.Quit();

		switch (step) {
		case Step.standby:
			Standby ();
			break;
		case Step.think:
			Think ();
			break;
		case Step.answer:
			Answer ();
			break;
		case Step.vote:
			Vote ();
			break;
		case Step.end:
			End ();
			break;
		}

		UpDataUIText ();

			
	}

	/*
	 * お題、使用数、アイテムのセット
	 * システムテキスト更新
	 */
	public void StandbyHead(){
		timesum = 0f;
		step = Step.standby;
		systemText.text =  playerDatas[order].GetName() + "さんの手番です";
		SetPinchNunberItemCard ();
	}


	/*
	 *アニメーションの終了待ち
	 *終わり次第Thinkへ移行
	 */
	public void Standby(){
		timesum += Time.deltaTime;
		if (timesum >= 4.0f) {
			ThinkHead ();
		}
	}

	/*
	 * システムテキスト更新
	 * 制限時間をセット
	 * アイテム欄を稼動状態に
	 */
	public void ThinkHead(){
		timesum = 0f;
		step = Step.think;
	}

	/*
	 * カウントアップ、システムテキストを更新
	 * アイテム欄の使用数状況をチェック、使用数と合っていれば解答ボタンを開放
	 * 解答ボタンを押されたらアイテム欄を停止させてアンサーへ
	 */
	public void Think(){
		timesum += Time.deltaTime;
		systemText.text = playerDatas[order].GetName() + "さん構想中　" + (int)timesum + "秒";
		if (timesum >= 2.0f) {
			AnswerHead ();
		}


	}

	/*
	 * システムテキスト更新
	 * 解答画面どーん
	 * 録音開始
	 */
	public void AnswerHead(){
		timesum = 0f;
		step = Step.answer;
	}

	/*
	 * カウントアップ、システムテキスト更新
	 * 解答終了ボタンが押されたら録音終了、解答画面とっぱらいVoteへ
	 */
	public void Answer(){
		timesum += Time.deltaTime;
		systemText.text = playerDatas[order].GetName() + "さん解答中　" + (int)timesum + "秒";
		if (timesum >= 2.0f) {
			VoteHead ();
		}

	}

	/*
	 * システムテキスト更新
	 * 投票画面リセットしてどーん
	 */
	public void VoteHead(){
		timesum = 0f;
		step = Step.vote;
		systemText.text = "投票中・・・";
	}

	/*
	 * 全員が投票できたら投票終了ボタン開放
	 * 投票終了ボタンが押されたら投票状況を記録してENdへ
	 */
	public void Vote(){
		timesum += Time.deltaTime;
		if (timesum >= 1.0f) {
			EndHead ();
		}
	}

	/*
	 * 合否アニメーション
	 */
	public void EndHead(){
		timesum = 0f;
		step = Step.end;
		systemText.text = "結果はOK!" + playerDatas[order].GetName() + "さんは1Pt獲得します";
	}

	/*
	 * 合否アニメーションが終わったらポイント変化、投票画面とっぱらい
	 * 使用したカードを破棄、新しいのをセット
	 * UI画面のノードを更新
	 * ボタンが押されたらフィールド上のカードをぽーい
	 * アニメーションが終わったら次の手番へ。　次の手番がなければラウンド数を増加させて先頭へ。次がなければゲーム終了フェイズへ
	 */public void End(){
		timesum += Time.deltaTime;
		if( 1f <= timesum && timesum <= 11f){
			systemText.text = "アイテムカードを補充します";
			timesum += 1000f;
		}
		if (1002f <= timesum && timesum <= 1012f) {
			TrashPinchNumItemCard();
			timesum += 1000f;
		}
		if(2004f <= timesum){
			order ++;
			if(order>= playerDatas.Count){
				order = 0;
				round ++;
			}
			StandbyHead();
		}
			
	}

	/*
	 * デバッグ用初期データ入力
	 */
	void InputInuOhaData(){
		InuOhaDatas.playerNames = dd_players;
		InuOhaDatas.useCards = dd_titles;
		InuOhaDatas.roundSetting = dd_round;
	}


	/*
	 * ＵＩ画面のラウンド数とプレイヤー数を更新する
	 */
	void UpDataUIText(){
		string temp;
		if (InuOhaDatas.roundSetting == InuOhaDatas.RoundSetting.Rondom)
			temp = "?";
		else if (InuOhaDatas.roundSetting == InuOhaDatas.RoundSetting.flee)
			temp = "∞";
		else
			temp = "" + maxRound;

		roundText.text = round + "/" + temp;

		playerText.text = playerDatas [order].GetName ();
	}


	/*
	 *InuOhaDatasからデータ読み出し
	 */
	void LoadInuOhaData(){
		playerNames = InuOhaDatas.playerNames;
		useCards = InuOhaDatas.useCards;
		roundSetting = InuOhaDatas.roundSetting;
	}

	void Initialize(){
		switch (roundSetting) {
		case InuOhaDatas.RoundSetting.thlee:
			maxRound = 3;
			break;
		case InuOhaDatas.RoundSetting.five:
			maxRound = 5;
			break;
		case InuOhaDatas.RoundSetting.Rondom:
			maxRound = Random.Range(2, 7);
			break;
		case InuOhaDatas.RoundSetting.flee:
			maxRound = 100000000;
			break;
		}
		round = 1;
		order = 0;
	
		playerDatas = new List<PlayerData> ();
		foreach (string s in playerNames) {
			PlayerData pd = new PlayerData ();
			pd.Initialize (s);
			playerDatas.Add (pd);
		}
			

		allPinchs = new List<string> ();
		allItems = new List<string> ();
		foreach (string s in useCards) {
			if (!AddCardsFromTitle (s)) 
				Debug.Log ("LoadCardError");
		}
		DebugContents ();
		pinchDeck = new List<string> (allPinchs);
		pinchGrave = new List<string> ();
		itemDeck = new List<string> (allItems);
		itemGrave = new List<string> ();

		//フィールド上のカードの座標取得
		pinchCardPoint = pinchCard.gameObject.GetComponent<RectTransform> ().position;
		numCardPoint = numCard.gameObject.GetComponent<RectTransform> ().position;
		for(int i=0;i<3;i++){
			itemCardPints[i] = itemCards[i].gameObject.GetComponent<RectTransform> ().position;
		}
			
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
			allPinchs.Add(line);
		}

		/*残りの行をアイテムに格納*/
		while (sr.EndOfStream != true) {
			line = sr.ReadLine ();
			if (line == string.Empty)
				continue;
			allItems.Add (line);
		}
		return true;
	}

	public void DebugContents(){
		string debugLine = "Pinchs;";
		foreach (string s in allPinchs) {
			debugLine += s + " ";
		}
		debugLine += "\nItems:";
		foreach (string s in allItems) {
			debugLine += s + " ";
		}
		Debug.Log (debugLine);
	}


	/*
	 * ピンチカードとナンバーカード、アイテムカードをセットします
	 */
	void SetPinchNunberItemCard(){
		//もしラウンド1ならアイテムをデッキからカードを選び、プレイヤーにもたせます。
		string[] items = playerDatas [order].GetItems ();
		bool[] isUsed = playerDatas [order].GetIsUsed ();
		if (round == 1) {
			for (int i = 0; i < 3; i++) {
				string drawItem = DrawItemCard ();
				items [i] = drawItem;
				isUsed [i] = false;
			}
		}
		//アイテムカードを表示上のカードに書き換えます。
		for (int i = 0; i < 3; i++) {
			itemCards [i].SetText (items [i]);
		}
		//次にナンバーカードとピンチカードを引き、表示上のカードを書き換えます。
		pinch = DrawPinchCard ();
		pinchCard.SetText (pinch);
		useNum = DrawNumCard ();
		numCard.SetText ("" + useNum);

		//表示上のカードのアニメーションをセットします
		Vector3 start;
		for(int i=0;i<3;i++){
			start = itemCardPints [i];
			start.x += 400 * i + 800;
			itemCards [i].Move (start, itemCardPints [i], 0.5f * i + 1f);
		}
		start = pinchCardPoint;
		start.x += 3200;
		pinchCard.Move (start, pinchCardPoint, 4f);
		start = numCardPoint;
		start.x += 3200;
		numCard.Move (start, numCardPoint, 4f);
	}



	/*
	 * ピンチカードを１枚引き、それを返します。
	 * 引くだけなので変数には代入されません。
	 */
	string DrawPinchCard(){
		//もしピンチデッキが空なら墓地をデッキに移し変えます
		if (pinchDeck.Count == 0) {
			pinchDeck = new List<string> (pinchGrave);
			pinchGrave.Clear ();
		}
		//ピンチデッキからランダムに１枚引きます。　引いたカードはデッキから外されます。　ここでは墓地には行きません。
		int pick = Random.Range(0, pinchDeck.Count);
		string draw = pinchDeck[pick];
		pinchDeck.RemoveAt (pick);
		return draw;
	}

	/*
	 * ナンバーカードを１枚引き、それを返します。
	 * 引くだけなので以下略
	 */
	int DrawNumCard(){
		return Random.Range (1, 4);
	}

	/*
	 * アイテムカードを１枚引き、それを渡します
	 * 引くだけ略
	 */
	string DrawItemCard(){
		//デッキにも墓地にもアイテムがない場合はもう１セットアイテムカードをデッキに追加します
		if(itemDeck.Count == 0 && itemGrave.Count == 0){
			itemDeck = new List<string> (allItems);
		}
		//デッキだけがない場合は墓地をデッキに移し変えます
		else if(itemDeck.Count == 0){
			pinchDeck = new List<string> (pinchGrave);
			pinchGrave.Clear ();
		}

		//アイテムデッキからランダムに１枚引きます。　引いたカードはデッキから外されます。　ここでは墓地には行きません。
		int pick = Random.Range(0, itemDeck.Count);
		string draw = itemDeck[pick];
		itemDeck.RemoveAt (pick);
		return draw;
	}

			
				

	void TrashPinchNumItemCard(){
		Vector3 goal = pinchCardPoint;
		goal.x -= 800;
		pinchCard.Move (goal, 1.0f);
		goal = numCardPoint;
		goal.x -= 800;
		numCard.Move (goal, 1.0f);
		for (int i = 0; i < 3; i++) {
			goal = itemCardPints [i];
			goal.x -= 800;
			itemCards [i].Move (goal, 1.0f);
		}

		pinchGrave.Add (pinchCard.GetText());
	}

}
