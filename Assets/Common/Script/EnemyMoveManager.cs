//================================================================================================================
// EnemyMoveManager.cs
// 製作者：戸軽隆二(八代くん流用)
//================================================================================================================
/* 概要
		ステージに存在する"Enemy"のtagを持ったオブジェクトの動きを制御する
		プレイヤーが近づくとプレイヤーにくっつく
*/
//================================================================================================================
// バージョン
//		1.0 クラス生成                              2017/03/08 戸軽隆二 
//		1.1 敵が強制的にPlayer1に集まる不具合修正	2017/03/09 戸軽隆二 
//================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveManager : MonoBehaviour {
	//---------------------------------------------------------------
	// private変数
	List<GameObject> players = new List<GameObject>();
	List<GameObject> enemys = new List<GameObject>();
	float limitDistance = 10f;	//敵キャラクターがどの程度近づいてくるか設定(この値以下には近づかない）
	float speed = 3;			//移動速度
	float frame = 0;            // 時間計測

	//================================================================================================================
	// 最初に１度だけ実行
	//================================================================================================================
	void Start() {
		GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < obj.Length; i++) players.Add(obj[i]);
	}
	//================================================================================================================
	// 常に更新し続ける処理
	//================================================================================================================
	void Update() {
		//--------------------------------------------------------------------
		// ステージ上の敵を検索
		if (frame >= 1.0f) {
			List<GameObject> enemylist = new List<GameObject>();
			GameObject[] obj = GameObject.FindGameObjectsWithTag("Enemy");
			for (int i = 0; i < obj.Length; i++) {
				//---------------------------------------------
				// プレイヤーにくっついた敵以外の情報にアクセス
				if (obj[i].transform.parent == null)
					enemylist.Add(obj[i]);
				else if(obj[i].transform.parent.tag != "Player")
					enemylist.Add(obj[i]);
			}

			enemys = enemylist;
		}
		else
			frame += Time.deltaTime;
		//------------------------------------------------------------------------------------------------------
		// プレイヤーが範囲内に近づいたらくっつく
		for (int i = 0; i < enemys.Count; i++) {
			for (int j = 0; j < players.Count; j++) {
				Vector3 playerPos = players[j].transform.position;                 //プレイヤーの位置
				Vector3 direction = playerPos - enemys[i].transform.position; //方向と距離を求める。
				float distance = direction.sqrMagnitude;            //directionから距離要素だけを取り出す。
				direction = direction.normalized;                   //単位化（距離要素を取り除く）
				direction.y = 0f;                                   //後に敵の回転制御に使うためY軸情報を消去。これにより敵上下を向かなくなる。

				//プレイヤーの距離が一定以下だと近づく
				if(distance < 1.0f) {
					enemys[i].transform.parent = players[j].transform;
				}
				else if (distance < limitDistance) {
					//プレイヤーとの距離が制限値以下なので近づく
					enemys[i].transform.position = enemys[i].transform.position + (direction * speed * Time.deltaTime);
				}
			}
		}
	}
}