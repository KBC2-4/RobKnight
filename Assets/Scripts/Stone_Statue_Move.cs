using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Statue_Move : MonoBehaviour
{

    public bool push_flg;
    public bool isAttacked = false; //攻撃を食らったのか
    public bool during_rotation = false; //回転中なのか


    // Start is called before the first frame update
    void Start()
    {
        push_flg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (push_flg == true)
        {
            transform.position += -transform.up * Time.deltaTime;
            push_flg = false;
        }
        if (during_rotation == true)
        {
            transform.Rotate(0f, 0f, 10 * Time.deltaTime); //回転
        }
    }

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
                    push_flg = true;
                    if (playerController.IsAttacking == true )
                    {
                        during_rotation = true;
                        isAttacked = false;
                    }


                }
            }
        }
    }
}
