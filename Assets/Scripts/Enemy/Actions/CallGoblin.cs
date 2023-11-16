//-------------------------------
//  ボスゴブリン：仲間呼び
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/CallGoblin")]
public class CallGoblin : EnemyAction
{

    public override void Act(EnemyController controller)
    {
        // 現在再生中のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("EndCall")) 
        {
            //モーションが終了してアニメーターがIdle状態になれば行動終了
            IsComplete = true;
            controller.animator.ResetTrigger("CallTrigger");
            return;
        }
        else if (stateInfo.IsName("Idle") && !IsComplete)
        {
            //仲間を呼ぶモーションのトリガーをセット
            controller.animator.SetTrigger("CallTrigger");
        }
    }
}


