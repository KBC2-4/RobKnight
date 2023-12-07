using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/AttackAction")]
public class AttackAction : EnemyAction
{
    public float attackRange = 2f;  //攻撃範囲
    public float attackCool = 0;    //攻撃間隔
    private float nowCool;

    public override void Act(EnemyController controller)
    {
        
        // 現在再生中のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
        
            controller.animator.SetFloat("Speed", 0);
        if (!stateInfo.IsName("Attack") && !stateInfo.IsTag("Attack"))
        {

            Vector3 direction = (controller.player.position - controller.transform.position).normalized;
            // エネミーがプレイヤーの方向を向く
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);

            //プレイヤー間の距離を取る
            float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);
            if (distanceToPlayer <= attackRange && nowCool <= 0)
            {
                // 攻撃アニメーションが再生中でないかつAttackRange内にプレイヤーがいる場合のみ、トリガーをセット
                controller.animator.SetTrigger("AttackTrigger");
            }

            if (5.0 <= ActionTime || IsComplete)
            {
                IsComplete = true;
            }
        }
        else 
        {
            nowCool = attackCool;
        }

        nowCool -= Time.deltaTime;
        if (nowCool < 0) nowCool = 0;
    }
}


