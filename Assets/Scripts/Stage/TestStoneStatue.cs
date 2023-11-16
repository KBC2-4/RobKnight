using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStoneStatue : MonoBehaviour
{
    /*
     * 各変数の宣言の仕方
    [Serializable] Int32 a = 3;　　private publicの中間
    private Int32 _b = 4;
    public Int32 c = 5;
    */

    public bool isAttacked = false; //攻撃を食らったのか
    public bool during_rotation = false; //回転中なのか

    private float _old_rotaey = 0;


    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    public float pushPower = 2.0F;

    void OnTriggerStay(Collider other)
    {
        // 衝突したオブジェクトがプレイヤーの場合
        if (other.gameObject.CompareTag("Player"))
        {
            // プレイヤーコントローラーを取得
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();    

            // プレイヤーが憑依している状態か確認します。
            if (playerController != null)
            {
                if (playerController.PossessionEnemyName == "Gobrin")
                {
                    if (playerController.IsAttacking == true && during_rotation == false)
                    {
                        _old_rotaey = transform.localEulerAngles.y;
                        during_rotation = true;
                        isAttacked = false;
                    }

                    if (during_rotation)
                    {
                        if (_old_rotaey + 15 >= transform.localEulerAngles.y)
                        {
                            transform.Rotate(0f, 0f, 10 * Time.deltaTime); //回転
                        }
                        else
                        {
                            during_rotation = false;
                        }
                    }
                }
            }
        }
    }
}