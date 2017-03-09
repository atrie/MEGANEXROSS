//================================================================================================================
// EnemyMoveManager.cs
// 製作者：戸軽隆二・八代恵輔
//================================================================================================================
/* 概要
		敵が一定の範囲内に入ってきたらプレイヤーにくっつく
		プレイヤーにくっついた敵は動かさない
*/
//================================================================================================================
// バージョン
//		1.0 クラス生成                               2017/03/09 戸軽隆二 
//================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveManager : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
    List<GameObject> enemys = new List<GameObject>();
    float limitDistance = 10f;	// 敵キャラクターがどの程度近づいてくるか設定(この値以下には近づかない）
    float speed = 3;			// 移動速度
    float frame = 0;            // 経過時間

	//================================================================================================================
	// 最初に１度だけ実行する処理
	//================================================================================================================
	void Start()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < obj.Length; i++) players.Add(obj[i]);
    }
	//================================================================================================================
	// 常に実行する処理
	//================================================================================================================
	void Update()
    {
        //--------------------------------------------------------------------
        // ステージ上の敵を検索
        if (frame >= 1.0f)
        {
			List<GameObject> list = new List<GameObject>();
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Enemy");
			for (int i = 0; i < obj.Length; i++) {
				//---------------------------------------------
				// プレイヤーと親子関係に合ったら検索から削除
				if (obj[i].transform.parent == null)
					list.Add(obj[i]);
				else if (obj[i].transform.parent.tag != "Player")
					list.Add(obj[i]);
			}
			enemys = list;
            frame = 0;
        }
        else
            frame += Time.deltaTime;
		//--------------------------------------------------------------------
		// 敵の範囲内にプレイヤーが来たらくっつく
		for (int i = 0; i < enemys.Count; i++)
        {
            for (int j = 0; j < players.Count; j++)
            {
                Vector3 playerPos = players[j].transform.position;                 //プレイヤーの位置
                Vector3 direction = playerPos - enemys[i].transform.position; //方向と距離を求める。
                float distance = direction.sqrMagnitude;            //directionから距離要素だけを取り出す。
                direction = direction.normalized;                   //単位化（距離要素を取り除く）
                direction.y = 0f;                                   //後に敵の回転制御に使うためY軸情報を消去。これにより敵上下を向かなくなる。

                //プレイヤーの距離が一定以下だと近づく
                if (distance < 1.0f)
                {
                    enemys[i].transform.parent = players[j].transform;
                }
                else if (distance < limitDistance)
                {
                    //プレイヤーとの距離が制限値以下なので近づく
                    enemys[i].transform.position = enemys[i].transform.position + (direction * speed * Time.deltaTime);
                }
            }
        }
    }
}