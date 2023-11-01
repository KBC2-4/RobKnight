using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/AttackAction")]
public class AttackAction : EnemyAction
{
    public float attackRange = 2f;

    public override void Act(EnemyController controller)
    {
        // if (controller == null || controller.player == null)
        // {
        //     Debug.LogError("ControllerまたはPlayerがnullです!");
        //     return;
        // }
        //
        // float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);
        // if (distanceToPlayer <= attackRange)
        // {
        //     Debug.Log("エネミー：攻撃モーション");
        //     controller.GetComponent<Animator>().SetBool("IsWalking", false);
        //     controller.GetComponent<Animator>().SetTrigger("AttackTrigger");
        // }
        
        controller.animator.SetBool("IsWalking", false);
        // 現在再生中のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName("Attack"))
        {
            // 攻撃アニメーションが再生中でない場合のみ、トリガーをセット
            controller.animator.SetTrigger("AttackTrigger");
        }
    }
}


