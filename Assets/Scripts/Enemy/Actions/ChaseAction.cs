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

        // 現在再生中のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        controller.animator.SetBool("IsWalking", true);

        if (rb != null)
        {
            if (controller && HasParameter(controller.animator, "RunTrigger"))
            {
                controller.animator.SetTrigger("RunTrigger");
            }

            Vector3 direction = (controller.player.position - controller.transform.position).normalized;
            // エネミーがプレイヤーの方向を向く
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            //controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);

            //移動モーション中ならエネミーを移動する
            if (stateInfo.IsName("Move01"))
            {
                // エネミーを移動させる
                Vector3 newPosition = rb.position + direction * (speed * Time.fixedDeltaTime);                
                rb.MovePosition(newPosition);

                // エネミーがプレイヤーの方向を向く
                controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);
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

