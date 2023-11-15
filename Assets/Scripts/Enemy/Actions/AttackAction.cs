using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/AttackAction")]
public class AttackAction : EnemyAction
{
    public float attackRange = 2f;

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

            // 攻撃アニメーションが再生中でない場合のみ、トリガーをセット
            controller.animator.SetTrigger("AttackTrigger");
        }
    }
}


