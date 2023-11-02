using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/WanderAction")]
public class WanderAction : EnemyAction
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 50f;

    public override void Act(EnemyController controller)
    {
        // 現在再生中のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
        controller.animator.SetFloat("Speed", moveSpeed);

        //死亡していない & アニメーターのステートがMove01なら移動処理に入る
        if (!controller.isDeath && stateInfo.IsName("Walk"))
        {
            controller.transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
            controller.transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));
            // !
        }
    }
}

