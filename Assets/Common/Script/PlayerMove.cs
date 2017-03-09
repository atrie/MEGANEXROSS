using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    private CharacterController charaCon;	// キャラクターコンポーネント用の変数
    private Vector3 move = Vector3.zero;    // キャラ移動量
    public float rotationSpeed = 10.0f;	// プレイヤーの回転速度
    private float inputXY;//左右入力変数
    private float inputXZ;//左右入力変数
    private float PlayerRotationY;//プレイヤーの回転変数
    private float PlayerRotationX;//プレイヤーの回転変数

    [SerializeField]private Transform target;//向く敵
    // Use this for initialization
    void Start()
    {
        Debug.Log("Hello, world!");
        charaCon = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AttractionMove.Instance.Move() == true)//体当り中は動けなくする
        {
            //*************:移動量の取得:***************//
            float y = move.y;
            move = new Vector3(inputXY, 0.0f, inputXZ);// 左右上下のキー入力を取得し、移動量に代入.
            Vector3 playerDir = move;   // 移動方向を取得.
            move *= 5;              // 移動速度を乗算.
            //*************:左右の移動:***************//
            inputXY = Input.GetAxis("Horizontal2");
            //ゲームパッドの十字キー右入力
            if (inputXY > 0)
            {
                //inputXY = 1;
                //Debug.Log("Right");
                //transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);

                PlayerRotationY += Time.deltaTime * 75;
                transform.rotation = Quaternion.Euler(0, PlayerRotationY, 0);
            }
            //ゲームパッドの十字キー左入力
            else if (inputXY < 0)
                {
                //inputXY = -1;
                //Debug.Log("Left");
                //transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
                PlayerRotationY += Time.deltaTime * -75;
                transform.rotation = Quaternion.Euler(0, PlayerRotationY, 0);
            }
            //何も入力されていない時
            else
            {
                inputXY = 0;
            }

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.Translate(Vector3.right * Time.deltaTime * 10 * 1);
            }
            else if (0 > Input.GetAxisRaw("Horizontal"))
            {
                transform.Translate(Vector3.left * Time.deltaTime * 10 * 1);
            }
            else
            {
                //左右方向には傾いていない
            }
            //*************:上下の移動:***************//
            inputXZ = Input.GetAxis("Vertical");
            //ゲームパッドの十字キー上入力
            if (inputXZ > 0)
            {
                //inputXZ = 1;
                //Debug.Log("Up");
                //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
                transform.Translate(Vector3.forward * Time.deltaTime * 10 * 1); //正面
            }
            //ゲームパッドの十字キー下入力
            else if (inputXZ < 0)
            {
                //inputXZ = -1;
                //Debug.Log("Down");
                //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
                transform.Translate(Vector3.forward * Time.deltaTime * -5 * 1); //正面
            }
            //何も入力されていない時
            else
            {
                inputXZ = 0;
            }
        }
        //*************:ゲームパッドのXボタン:***************//
        if (Input.GetButtonUp("Fire1"))
        {
            Debug.Log("GO!!");
            //float step = 100.0f * Time.deltaTime;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, step);
            AttractionMove.Instance.Attraction();//プレイヤー同士をぶつける処理
            transform.LookAt(target);
            Vector3 rote = transform.eulerAngles;
            PlayerRotationY = rote.y;
            transform.eulerAngles = rote;
        }
        //*************:プレイヤーの向き変更:***************//
        //if (playerDir.magnitude > 0.1f)
        //{
        //    Quaternion q = Quaternion.LookRotation(playerDir);//向きたい方角をQuaternionn型に直す .
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);// 向きを q に向けてじわ～っと変化させる.
        //}
    }
}
