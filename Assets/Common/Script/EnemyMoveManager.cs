using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveManager : MonoBehaviour
{

    List<GameObject> players = new List<GameObject>();
    List<GameObject> enemys = new List<GameObject>();
    private float limitDistance = 10f; //敵キャラクターがどの程度近づいてくるか設定(この値以下には近づかない）
    private bool on = true;
    private float speed = 3; //移動速度

    float frame = 0;

    void Start()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < obj.Length; i++) players.Add(obj[i]);
    }

    void Update()
    {
        //--------------------------------------------------------------------
        // ステージ上の敵を検索
        if (frame >= 1.0f)
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < obj.Length; i++) enemys.Add(obj[i]);
            frame = 0;
        }
        else
            frame += Time.deltaTime;

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