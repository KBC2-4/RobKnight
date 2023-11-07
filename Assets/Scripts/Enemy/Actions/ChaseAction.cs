using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/ChaseAction")]
public class ChaseAction : EnemyAction
{
    public float speed = 5f;

    public override void Act(EnemyController controller)
    {
        Chase(controller);
    }

    private void Chase(EnemyController controller)
    {
        
        Rigidbody rb = controller.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 現在再生中のアニメーションの状態を取得
            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
            
            
            //移動モーション中ならエネミーを移動する
            if (stateInfo.tagHash == Animator.StringToHash("Move"))
            {

                Vector3 direction = (controller.player.position - controller.transform.position).normalized;
                // エネミーがプレイヤーの方向を向く
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                // スムーズに回転させる
                controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);
                
                // エネミーを移動させる
                Vector3 newPosition = rb.position + direction * (speed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
            }
            else
            {
                // 歩くアニメーションを再生
                controller.animator.SetFloat("Speed", speed);
                
                // 走るモーションがある場合は、走るモーションを使用する
                if (controller && HasParameter(controller.animator,"RunTrigger"))
                {
                    controller.animator.SetTrigger("RunTrigger");
                }
            }
        }
    }
    public bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }
}