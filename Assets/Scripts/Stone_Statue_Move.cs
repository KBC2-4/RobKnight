using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Statue_Move : MonoBehaviour
{
    public button Button;
    public float power = 0.5f;

    public bool push_flg;
    public bool isAttacked = false; //攻撃を食らったのか
    //int count; //押されている間＋されていく

    // Start is called before the first frame update
    void Start()
    {
        push_flg = false;
        //count = 0;
    }

    private Vector3 force = new Vector3(0, 0, 0);   //押し出す力
    private Vector3 forcedecay = new Vector3(0, 0, 0);   //押し出す力の減衰
    private float forcetime = 0;                      //押し出す時間
    // Update is called once per frame
    void Update()
    {
        if (push_flg == true)
        {
            transform.position += -transform.up * Time.deltaTime;
            push_flg = false;
            //count = 0;
        }

        if(!Button.push_flg) transform.position += force;

        //押し出す時間と力を減らす
        forcetime -= Time.fixedDeltaTime;
        if (forcetime < 0)
        {
            force = Vector3.zero;
            forcedecay = Vector3.zero;
            forcetime = 0;
        }
        else
        {
            force -= (forcedecay * Time.fixedDeltaTime);
        }

        transform.position = (new Vector3(Mathf.Clamp(transform.position.x, 347, 372), transform.position.y, Mathf.Clamp(transform.position.z, 707, 729)));
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
                    //count++;
                    if (playerController.IsAttacking == true)
                    {
                        //playerController.transform.rotation = Quaternion.identity

                        Vector3 PlayerPos = playerController.transform.position;
                        Vector3 NowPos = transform.position;

                        PlayerPos.y = 0;
                        NowPos.y = 0;
                        
                        //プレイヤーと対象間の角度を取る
                        var diff = (NowPos - PlayerPos).normalized;
                        Vector3 PushAngle = diff * power;

                        force = PushAngle;
                        forcedecay = PushAngle;
                        forcetime = 1;

                        isAttacked = false;




                        //count = 0;
                    }

                    //if (count >= 5)
                    //{
                    //    push_flg = true;
                    //}
                }
            }
        }
    }
}
