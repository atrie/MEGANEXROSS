//================================================================================================================
// AttractionMove.cs
// 製作者：戸軽隆二
//================================================================================================================
/* 概要
		磁力で引き合う際の動きを実装したスクリプト。
		空のゲームオブジェクトを生成しそこにスクリプトをアタッチ(例 名前はAttractionManagerとか)
		後はInspectorから速さだけ調節でき、処理を実行したいときは
		Attraction.Instance.Attraction();	の関数を呼び出せばOK。
		Singletonクラスを継承しているのでどのスクリプトからも呼び出せる。
*/		
//================================================================================================================
// バージョン
//		1.0 クラス生成                               2017/03/08 戸軽隆二 
//================================================================================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttractionMove : SingletonMonoBehaviour<AttractionMove> {
	//---------------------------------------------------------------
	// private変数
	List<GameObject> players = new List<GameObject>();
	List<Vector3> startPos = new List<Vector3>();
	Vector3 gorlPos = new Vector3();
	bool moveFlg = false;
	float velocity = 0;
	//---------------------------------------------------------------
	// public変数
	public float speed = 1.0f;

	//================================================================================================================
	// 最初に１度だけ実行する処理
	//================================================================================================================
	void Start () {
		// プレイヤーの情報にアクセス
		GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < obj.Length; i++) {
			players.Add(obj[i]);
		}
	}
	//================================================================================================================
	// 常に更新し続ける処理（60FPSで固定）
	//================================================================================================================
	void FixedUpdate() {
		//-------------------------------------------------------
		// 引き合う時の処理
		if (moveFlg == true) {
			for (int i = 0; i < players.Count; i++) {
				Vector3 pos = players[i].transform.position;
				Vector3 dis = gorlPos - startPos[i];
                pos += (dis * Time.deltaTime) * velocity;
				//----------------------------------------------------
				// 一定の距離まで近づくまで動く
				if (Vector3.Distance(pos, gorlPos) >= 0.5f)
					players[i].transform.position = pos;
				else {
					ReactionStart();
					VibrationCamera.Instance.Vibration();
					List<GameObject> children1 = GetAllChildren.GetAll(players[0]);
					List<GameObject> children2 = GetAllChildren.GetAll(players[1]);
					//------------------------------------------------------
					// 親子関係にある敵だけ削除
					for (int num = 0; num < children1.Count; num++)
						if(children1[num].tag == "Enemy")
							Destroy(children1[num]);
					for (int num = 0; num < children2.Count; num++)
						if (children2[num].tag == "Enemy")
							Destroy(children2[num]);
					break;
				}
			}
			velocity += speed/ 100;
		}
	}
	//================================================================================================================
	// 衝突した時の反動のスクリプトを実行
	//================================================================================================================
	void ReactionStart() {
		velocity = 0;
		moveFlg = false;
		startPos = new List<Vector3>();
		gorlPos = new Vector3();
		GetComponent<ReactionMove>().Reaction(players);
	}
	//================================================================================================================
	// メソッドを呼び出せばお互いに磁力で引き合う
	//================================================================================================================
	public void Attraction() {
		moveFlg = true;
		//-------------------------------------------------
		// 中間座標を計算
		Vector3 p1Pos = players[0].transform.position;
		Vector3 p2Pos = players[1].transform.position;
		gorlPos = p1Pos - (p1Pos - p2Pos) / 2;
		// プレイヤーの最初の座標を覚える
		startPos.Add(p1Pos);
		startPos.Add(p2Pos);
	}
	//================================================================================================================
	// AttractionとReactonの２つが動いてない時のフラグを取得する
	//================================================================================================================
	public bool Move() {
		bool moveFlg2 = GetComponent<ReactionMove>().MoveFlg;
		if (moveFlg == false && moveFlg2 == false)
			return true;
		else
			return false;
	}
}
