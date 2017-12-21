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
	[SerializeField] PlayerUI playerUI = null;
	[SerializeField] Button nextButton = null;
	[SerializeField] Text nextButtonText = null;
	[SerializeField] VotePanel votePanel = null;
	[SerializeField] OkNoAnime okNoAnime = null;

	public enum Step{standby,think,answer,vote,end,finish};
	Step step;
	float timesum;

	int maxRound,round;
	int order;

	bool[] endFlags = new bool[5];

	List<string> playerNames;
	List<CardData> cardDatas;
	InuOhaDatas.RoundSetting roundSetting;
	List<PlayerData> playerDatas;
	List<string> allPinchs,pinchDeck,pinchGrave;
	List<string> allItems,itemDeck,itemGrave;

	/*ピンチカードとナンバーカードの内容*/
	string pinch;
	int useNum;

	[SerializeField] ItemZone itemZone = null;


	/* デバッグ関連*/
	public bool isUseDebugData = false;
	public List<string> dd_players;
	public List<string> dd_cardFileNames;
	public InuOhaDatas.RoundSetting dd_round;
	public float defaultTimeScale = 1.0f;

	bool judge;

	/*サウンド*/
	AudioSource bgmSound,finishSound;
	[SerializeField] AudioManager audioManager;


	void Awake(){
		if(isUseDebugData) InputInuOhaData ();
		Time.timeScale = defaultTimeScale;
	}

	// Use this for initialization
	void Start () {
		LoadInuOhaData ();
		Initialize ();
		InitializeUI ();
		itemZone.Initialize ();
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
		case Step.finish:
			Finish ();
			break;
		}

		UpDataUIText ();

			
	}

	/*
	 * お題、使用数、アイテムのセット
	 * システムテキスト更新
	 */
	public void StandbyHead(){
		//DebugMessageWindw.SetMessage ("Stanby");
		timesum = 0f;
		step = Step.standby;
		systemText.text =  playerDatas[order].GetName() + "さんの手番です";
		SetPinchNunberItemCard ();

		//次へボタンを更新。　現状オフ。
		nextButton.interactable = false;
		nextButtonText.text = "整いました！";
		//アイテムゾーンの使用状況を初期化
		itemZone.Initialize();

	}


	/*
	 *アニメーションの終了待ち
	 *終わり次第Thinkへ移行
	 */
	public void Standby(){
		timesum += Time.deltaTime;
		if (timesum >= 3.5f) {
			ThinkHead ();
		}
	}

	/*
	 * システムテキスト更新
	 * 制限時間をセット
	 * アイテム欄を稼動状態に
	 */
	public void ThinkHead(){
		//DebugMessageWindw.SetMessage ("Think");
		timesum = 0f;
		step = Step.think;
		itemZone.SetIsChoiceable (true);
	}

	/*
	 * カウントアップ、システムテキストを更新
	 * アイテム欄の使用数状況をチェック、使用数と合っていれば解答ボタンを開放
	 */
	public void Think(){
		timesum += Time.deltaTime;
		if (itemZone.GetChoiceCount () == useNum) {
			nextButton.interactable = true;
		} else {
			nextButton.interactable = false;
		}
		systemText.text = playerDatas[order].GetName() + "さん考え中　" + (int)timesum + "秒";
	}

	/*
	 * システムテキスト更新
	 * 解答画面どーん
	 * 録音開始
	 */
	public void AnswerHead(){
		//DebugMessageWindw.SetMessage ("Answer");
		timesum = 0f;
		step = Step.answer;
		itemZone.SetIsChoiceable (false);
		//次へボタンを更新。　誤操作防止のため、はじめはオフに。　後でオンします。
		nextButton.interactable = false;
		nextButtonText.text = "回答終了";
		audioManager.FadeOut (3.0f);
	}

	/*
	 * カウントアップ、システムテキスト更新
	 * 解答終了ボタンが押されたら録音終了、解答画面とっぱらいVoteへ
	 */
	public void Answer(){
		timesum += Time.deltaTime;
		systemText.text = playerDatas[order].GetName() + "さん回答中　" + (int)timesum + "秒";
		if (timesum >= 1.0f) {
			nextButton.interactable = true;
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
		/*VotePanelの準備*/
		List<string> voters = new List<string> ();
		for (int i = 0; i < playerDatas.Count; i++) {
			if (i != order) {
				voters.Add (playerDatas [i].GetName ());
			}
		}
		votePanel.Initialize (voters);
		votePanel.gameObject.gameObject.GetComponent<CallAndBye> ().Call ();
		//次へボタンを更新。　はじめはオフに。　後でオンします。
		nextButton.interactable = false;
		nextButtonText.text = "投票終了";
		audioManager.FadeIn (1.0f);
	}

	/*
	 * 全員が投票できたら投票終了ボタン開放
	 * 投票終了ボタンが押されたら投票状況を記録してENdへ
	 */
	public void Vote(){
		if (votePanel.GetChoicedNum () == votePanel.GetVoterNum())
			nextButton.interactable = true;
		else
			nextButton.interactable = false;
	}

	/*
	 * 合否アニメーション
	 */
	public void EndHead(){
		timesum = 0f;
		step = Step.end;
		judge = votePanel.GetIsOk ();
		okNoAnime.gameObject.GetComponent<CallAndBye> ().Call ();
		okNoAnime.Set (judge,1f);
		nextButton.interactable = false;

		for (int i = 0; i<endFlags.Length;i++) {
			endFlags[i] = false;
		}
	}

	/*
	 * 合否アニメーションが終わったらポイント変化、投票画面とっぱらい
	 * 使用したカードを破棄、新しいのをセット
	 * UI画面のノードを更新
	 * ボタンが押されたらフィールド上のカードをぽーい
	 * アニメーションが終わったら次の手番へ。　次の手番がなければラウンド数を増加させて先頭へ。次がなければゲーム終了フェイズへ
	 */

	public void End(){
		timesum += Time.deltaTime;
		//合否アニメ終了時。　判定結果の反映
		if( timesum >= 2.0f && !endFlags[0]){
			endFlags [0] = true;
			Debug.Log(endFlags[0]);
			votePanel.gameObject.GetComponent<CallAndBye> ().Bye ();
			votePanel.Clean ();
			okNoAnime.gameObject.GetComponent<CallAndBye> ().Bye ();
			if (judge) {
				systemText.text = "結果はOK!" + playerDatas [order].GetName () + "さんは1Pt獲得します";
			}
			else{
				systemText.text = "結果はNO...";
			}
			playerDatas [order].AddIsOk (judge);

		}
		//アイテムカードの交換始動・　テキスト更新、使われたカードを下へ動かす
		if (timesum >= 3.5f && !endFlags [1]) {
			endFlags[1]= true;
			systemText.text = "アイテムカードを交換します";
			bool[] isUse = itemZone.GetChoiceStatus ();
			bool soundFlag = false;
			for (int i = 0; i < 3; i++) {
				if (isUse [i]) {
					soundFlag = true;
					Vector3 goal = itemCardPints [i];
					goal.y -= 600;
					itemCards [i].Move (goal, 0.8f);
				}
			}
			if (soundFlag)
				itemCards [0].SetSound (Card.SoundType.Slide,0);
		}
		//下へ行くアニメの終了時。　そのアイテムを墓地に送り、新たなアイテムを引き、上から下ろしてくる。
		if (timesum >= 4.5f && !endFlags[2]){
			endFlags [2] = true;
			bool[] isUse = itemZone.GetChoiceStatus ();
			string[] items = playerDatas [order].GetItems ();
			bool soundFlag = false;
			for (int i = 0; i < 3; i++) {
				if (isUse [i]) {
					soundFlag = true;
					itemGrave.Add (items [i]);
					items [i] = DrawItemCard ();
					itemCards [i].SetText (items [i]);
					Vector3 start = itemCardPints [i];
					start.y += 600;
					itemCards [i].Move (start, itemCardPints [i], 0.8f);
				}
			}
			if (soundFlag)
				itemCards [0].SetSound (Card.SoundType.Slide,0);
		}
		//アイテム補充アニメ終了時。　ボタンの押し待ちに入るので一旦時を止めます。
		if (timesum >= 5.5f && !endFlags [3]) {
			endFlags [3] = true;
			systemText.text = "ボタンを押すと次の手番へ進みます";
			nextButtonText.text = "手番終了";
			nextButton.interactable = true;
			Time.timeScale = 0;
		}
		//次へボタンが押されたらフラグが立って入ります。時をまた進めます。
		if (endFlags [4]) {
			endFlags[4] = false;
			Time.timeScale = defaultTimeScale;
			nextButton.interactable = false;
			TrashPinchNumItemCard ();
		}

		if (timesum >= 7.0f) {
			//まだ次の手番があるなら手番を進めます。
			if (order < playerDatas.Count - 1) {
				order++;
			}
			//ないならラウンドが進みます
			else {
				//まだ次のラウンドがあるならラウンドを進めて手番をはじめに戻します
				if (round < maxRound) {
					round++;
					order = 0;
				}
				//ここまでくるとゲーム終了です
				else {
					FinishHead ();
					return;
				}
			}
			StandbyHead ();	
		}
			

			
	}

	void FinishHead(){
		step = Step.finish;
		timesum = 0f;
		systemText.text = "ゲーム終了!";
		bgmSound.mute = true;
		finishSound.PlayOneShot (finishSound.clip);
	}

	void Finish(){
		timesum += Time.deltaTime;
		if (timesum >= 3.0f) {
			InuOhaDatas.records = MakeRecords ();
			SceneManager.LoadScene("Result");
		}
	}

	/*
	 * デバッグ用初期データ入力
	 */
	void InputInuOhaData(){
		InuOhaDatas.playerNames = dd_players;
		foreach (string s in dd_cardFileNames) {
			CardData cd = new CardData ();
			if (cd.LoadFromFile (s))
				InuOhaDatas.useCards.Add (cd);
			else
				Debug.Log ("FileError:" + s);
		}
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
		cardDatas = InuOhaDatas.useCards;
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
		foreach (CardData cd in cardDatas) {
			AddCardsFromCardData (cd);
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

		AudioSource[] souces = GetComponents<AudioSource> ();
		bgmSound = souces [0];
		finishSound = souces [1];
			
	}

	public void AddCardsFromCardData(CardData cardData){
		allPinchs.AddRange (cardData.GetPinchs ());
		allItems.AddRange (cardData.GetItems ());
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
			itemCards [i].Move (start, itemCardPints [i], 0.4f * i + 0.8f);
			itemCards [i].SetSound (Card.SoundType.Slide,0.4f * i);
		}
		start = pinchCardPoint;
		start.x += 3200;
		pinchCard.Move (start, pinchCardPoint, 3.2f);
		start = numCardPoint;
		start.x += 3200;
		numCard.Move (start, numCardPoint, 3.2f);
		pinchCard.SetSound (Card.SoundType.Slide,2.4f);
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
		//Debug.Log ("deck:" + itemDeck.Count + " grave:" + itemGrave.Count);
		//デッキにも墓地にもアイテムがない場合はもう１セットアイテムカードをデッキに追加します
		if(itemDeck.Count == 0 && itemGrave.Count == 0){
			itemDeck = new List<string> (allItems);
		}
		//デッキだけがない場合は墓地をデッキに移し変えます
		else if(itemDeck.Count == 0){
			itemDeck = new List<string> (itemGrave);
			itemGrave.Clear ();
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
		pinchCard.Move (goal, 0.8f);
		goal = numCardPoint;
		goal.x -= 800;
		numCard.Move (goal, 0.8f);
		for (int i = 0; i < 3; i++) {
			goal = itemCardPints [i];
			goal.x -= 800;
			itemCards [i].Move (goal, 0.8f);
		}
		pinchGrave.Add (pinchCard.GetText());
		pinchCard.SetSound (Card.SoundType.Slide,0f);
		numCard.SetSound (Card.SoundType.Slide,0.1f);
		itemCards [0].SetSound (Card.SoundType.Slide,0.2f);

	}


	/*
	 * UI関連
	 */
	void InitializeUI(){
		playerUI.Initialize (playerDatas);
	}
	void UpdateUI(){
		playerUI.UpdateDisplay ();
	}
		
	public void PushNextButton(){
		if (step == Step.think) {
			AnswerHead ();
		} else if (step == Step.answer) {
			VoteHead ();
		} else if (step == Step.vote) {
			EndHead ();
		} else if (step == Step.end) {
			endFlags [4] = true;
		}
	}

	public List<Record> MakeRecords(){
		List<Record> records = new List<Record> ();
		foreach (PlayerData pd in playerDatas) {
			records.Add (pd.GetRecord ());
		}
		return records;
	}
}
