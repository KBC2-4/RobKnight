using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーとの距離によって距離を取ったり近づいたりしながら攻撃する

[CreateAssetMenu(menuName = "EnemyActions/MoveAndAttack")]
public class MoveAndAttack : EnemyAction
{
    public float attackRange = 2f;  //攻撃を開始する範囲

    public float BackSpeed = 1f;    //離れる速度
    public float BackBorder = 1f;   //離れる移動を開始する距離

    public float CloserSpeed = 1f;  //接近する速度
    public float CloserBorder = 4f; //接近を開始する距離

    public float AttackCool = 1f;   //攻撃間隔
    private float NowCool = 1f;   //攻撃間隔

    public override void Act(EnemyController controller)
    {
        
        // 現在再生中のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        //プレイヤー間の距離を取る
        float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);

        if (!stateInfo.IsName("Attack") && !stateInfo.IsTag("Attack"))
        {

            Vector3 direction = (controller.player.position - controller.transform.position).normalized;
            // エネミーがプレイヤーの方向を向く
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);

            if (distanceToPlayer <= attackRange && NowCool <= 0)      //攻撃を開始する
            {
                // 攻撃アニメーションが再生中でないかつAttackRange内にプレイヤーがいる場合のみ、トリガーをセット
                controller.animator.SetTrigger("AttackTrigger");
            }
            else    //移動する
            {

                CharacterController Cc = controller.GetComponent<CharacterController>();

                if (distanceToPlayer <= BackBorder) //離れる移動
                {
                    if (Cc != null)
                    {
                        // エネミーを移動させる
                        Vector3 newPosition = direction * (BackSpeed * Time.fixedDeltaTime);
                        if (stateInfo.IsName("Walk")) Cc.Move(-newPosition);
                        controller.animator.SetTrigger("WalkTrigger");
                        controller.animator.SetBool("IsEndWalk", false);
                        controller.animator.ResetTrigger("AttackTrigger");
                    }
                }
                else if (CloserBorder <= distanceToPlayer) //接近する移動
                {
                    if (Cc != null)
                    {
                        // エネミーを移動させる
                        Vector3 newPosition = direction * (CloserSpeed * Time.fixedDeltaTime);
                        if (stateInfo.IsName("Walk")) Cc.Move(newPosition);
                        controller.animator.SetTrigger("WalkTrigger");
                        controller.animator.SetBool("IsEndWalk", false);
                        controller.animator.ResetTrigger("AttackTrigger");
                    }
                }
                else 
                {
                    controller.animator.SetBool("IsEndWalk", true);
                }
            }

            if (5.0 <= ActionTime || IsComplete)
            {
                IsComplete = true;
            }
        }
        else
        {
            //攻撃クールタイムをセット
            NowCool = AttackCool;
        }

        NowCool -= Time.fixedDeltaTime;
        if (NowCool < 0) NowCool = 0;
    }
}


