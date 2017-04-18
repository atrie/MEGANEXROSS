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
//		2.0 処理を自機だけに変更					 2017/04/18 戸軽隆二
//================================================================================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttractionMove : SingletonMonoBehaviour<AttractionMove> {
	//---------------------------------------------------------------
	// private変数
	List<GameObject> objects = new List<GameObject>();
	Vector3 startPos, gorlPos;
	bool moveFlg = false;
	float velocity = 0;
	//---------------------------------------------------------------
	// public変数
	public float speed = 1.0f;

	//================================================================================================================
	// 常に更新し続ける処理（60FPSで固定）
	//================================================================================================================
	void FixedUpdate() {
		//-------------------------------------------------------
		// 引き合う時の処理
		if (moveFlg == true) {
			GameObject player = objects[0];
            Vector3 pos = player.transform.position;
			Vector3 dis = gorlPos - startPos;
            pos += (dis * Time.deltaTime) * velocity;
			//----------------------------------------------------
			// 一定の距離まで近づくまで動く
			if (Vector3.Distance(pos, gorlPos) >= 0.5f)
				player.transform.position = pos;
			else {
				ReactionStart();
				VibrationCamera.Instance.Vibration();
				List<GameObject> children1 = GetAllChildren.GetAll(player);
				//------------------------------------------------------
				// 親子関係にある敵だけ削除
				for (int num = 0; num < children1.Count; num++)
					if(children1[num].tag == "Enemy")
						Destroy(children1[num]);
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
		startPos = new Vector3();
		gorlPos = new Vector3();
		GetComponent<ReactionMove>().Reaction(objects);
	}
	//================================================================================================================
	// メソッドを呼び出せばお互いに磁力で引き合う
	//================================================================================================================
	public void Attraction(GameObject player, GameObject target) {
		moveFlg = true;
		//-------------------------
		// オブジェクトの情報を保存
		objects.Add(player);
		objects.Add(target);
		//-----------------------------------
		// スタート座標と目的の座標を保存
		startPos = player.transform.position;
		gorlPos = target.transform.position;
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
