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
	public Card testCard = null;
	public enum Step{standby,think,answer,vote,end};
	Step step;
	float timesum;
	int maxRound,round;
	int order;
	string justPlayerName;

	/* デバッグ用初期データ入力群*/
	public bool isUseDebugData = false;
	public List<string> dd_players;
	public List<string> dd_titles;
	public InuOhaDatas.RoundSetting dd_round;

	// Use this for initialization
	void Start () {
		InputInuOhaData ();
		StandbyHead ();
		timesum = 0f;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("escape"))
			Application.Quit();

		if (!testCard.GetisMoving()) {
			testCard.Move (new Vector3(800,300,0),new Vector3(0,300,0), 1.0f);
		}

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

			
	}

	/*
	 * お題、使用数、アイテムのセット
	 * システムテキスト更新
	 */
	public void StandbyHead(){
		step = Step.standby;
		systemText.text =  "プレイヤー1さんの手番です";
	}


	/*
	 *アニメーションの終了待ち
	 *終わり次第Thinkへ移行
	 */
	public void Standby(){
		timesum += Time.deltaTime;
		if (timesum >= 3.0f) {
			timesum = 0f;
			ThinkHead ();
		}
	}

	/*
	 * システムテキスト更新
	 * 制限時間をセット
	 * アイテム欄を稼動状態に
	 */
	public void ThinkHead(){
		step = Step.think;
	}

	/*
	 * カウントアップ、システムテキストを更新
	 * アイテム欄の使用数状況をチェック、使用数と合っていれば解答ボタンを開放
	 * 解答ボタンを押されたらアイテム欄を停止させてアンサーへ
	 */
	public void Think(){
		timesum += Time.deltaTime;
		systemText.text = "プレイヤー1さん構想中　" + (int)timesum + "秒";
		if (timesum >= 5.0f) {
			timesum = 0f;
			AnswerHead ();
		}


	}

	/*
	 * システムテキスト更新
	 * 解答画面どーん
	 * 録音開始
	 */
	public void AnswerHead(){
		step = Step.answer;
	}

	/*
	 * カウントアップ、システムテキスト更新
	 * 解答終了ボタンが押されたら録音終了、解答画面とっぱらいVoteへ
	 */
	public void Answer(){
		timesum += Time.deltaTime;
		systemText.text = "プレイヤー1さん解答中　" + (int)timesum + "秒";
		if (timesum >= 5.0f) {
			timesum = 0f;
			VoteHead ();
		}

	}

	/*
	 * システムテキスト更新
	 * 投票画面リセットしてどーん
	 */
	public void VoteHead(){
		step = Step.vote;
		systemText.text = "投票中・・・";
	}

	/*
	 * 全員が投票できたら投票終了ボタン開放
	 * 投票終了ボタンが押されたら投票状況を記録してENdへ
	 */
	public void Vote(){
		timesum += Time.deltaTime;
		if (timesum >= 3.0f) {
			timesum = 0f;
			EndHead ();
		}
	}

	/*
	 * 合否アニメーション
	 */
	public void EndHead(){
		step = Step.end;
		systemText.text = "結果はOK!プレイヤー１さんは1Pt獲得します";
	}

	/*
	 * 合否アニメーションが終わったらポイント変化、投票画面とっぱらい
	 * 使用したカードを破棄、新しいのをセット
	 * UI画面のノードを更新
	 * ボタンが押されたらフィールド上のカードをぽーい
	 * アニメーションが終わったら次の手番へ。　次の手番がなければラウンド数を増加させて先頭へ。次がなければゲーム終了フェイズへ
	 */public void End(){
		timesum += Time.deltaTime;
		if(timesum>=2.0f){
			systemText.text = "アイテムカードを補充します";
		}
		if (timesum >= 4.0f) {
			timesum = 0f;
			StandbyHead ();
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
	void UpDataUIText(){
		string temp;
		if(InuOhaDatas.
*/
}
