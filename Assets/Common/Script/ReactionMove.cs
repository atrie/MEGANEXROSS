//================================================================================================================
// ReactionMove.cs
// 製作者：戸軽隆二
//================================================================================================================
/* 概要
		AttractionMoveの実行後に呼ばれるスクリプト。
		衝突した時の反動の動きを実装している。
		AttractionMoveがアタッチされているオブジェクトにこのスクリプトをアタッチする
*/
//================================================================================================================
// バージョン
//		1.0 クラス生成                               2017/03/08 戸軽隆二 
//================================================================================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReactionMove : MonoBehaviour {
	//---------------------------------------------------------------
	// private変数
	List<GameObject> players = new List<GameObject>();
	List<Vector3> startPos = new List<Vector3>();
	Vector3 gorlPos;
	bool moveFlg = false;
	float frame = 0;
	//---------------------------------------------------------------
	// public変数
	public float time = 0.6f;

	//================================================================================================================
	// 常に更新し続ける処理（60FPSで固定）
	//================================================================================================================
	void FixedUpdate () {
		if (moveFlg == true) {
			if(frame < time) {
				frame += Time.deltaTime;
				for (int i = 0; i < players.Count; i++) {
					Vector3 pos = players[i].transform.position;
					Vector3 dis = gorlPos - startPos[i];
					pos = startPos[i] - (dis * (frame / time) );
					pos.y = startPos[i].y + ( (-1.0f + (frame / time) * 2) * (-1.0f + (frame / time) * 2) ) * -1 + 1;
					players[i].transform.position = pos;
				}
			}
			else {
				Initialize();
			}
		}
	}
	//================================================================================================================
	// 初期化の処理
	//================================================================================================================
	void Initialize() {
		frame = 0;
		moveFlg = false;
		for (int i = 0; i < players.Count; i++) {
			Vector3 pos = players[i].transform.position;
			pos.y = startPos[i].y;
			players[i].transform.position = pos;
		}
		players = new List<GameObject>();
		startPos = new List<Vector3>();
	}
	//================================================================================================================
	// メソッドを呼び出せば反動の処理を実行できる
	//================================================================================================================
	public void Reaction(List<GameObject> player) {
		moveFlg = true;     // Update実行
		players = player;	// プレイヤーの情報を保存
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
	// プロパティ関数
	//================================================================================================================
	public bool MoveFlg { get { return moveFlg; } set { moveFlg = value; } }
}
