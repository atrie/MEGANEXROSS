using UnityEngine;
using System.Collections;

public class EnemySet : MonoBehaviour {

    //プレハブを変数に代入
    [SerializeField]private GameObject EnemyPre;
    private float intervalTime = 0.0f;//秒ごとの変数
    private int Enemyint = 0;//敵の数
    void Start()
    {
        ////オブジェクトの座標
        //float x = Random.Range(-10.0f, 10.0f);
        //float y = -2;
        //float z = Random.Range(-10.0f, 10.0f);

        ////オブジェクトを生産
        //Instantiate(EnemyPre, new Vector3(x, y, z), Quaternion.identity);
    }
    void Update()
    {
        intervalTime += Time.deltaTime;
        if (intervalTime >= 0.1f)
        {
            //オブジェクトの座標
            float x = Random.Range(-10.0f, 10.0f);
            float y = -2;
            float z = Random.Range(-10.0f, 10.0f);
            //オブジェクトを生産
            Instantiate(EnemyPre, new Vector3(x, y, z), Quaternion.identity);
            //敵の数を数える
            Enemyint++;
            //デバッグログ
            Debug.Log(Enemyint);
            //上がった数を0に
            intervalTime = 0.0f;
        }
    }
}
