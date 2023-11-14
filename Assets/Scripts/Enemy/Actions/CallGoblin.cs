//-------------------------------
//  ボスゴブリン：仲間呼び
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/CallGoblin")]
public class CallGoblin : EnemyAction
{
    private bool IsCalled = false;

    public override void Act(EnemyController controller)
    {
        // 現在再生中のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        Debug.Log($"State:{IsCalled}");

        if (!IsCalled)
        {
            //仲間を呼ぶモーションのトリガーをセット
            controller.animator.SetTrigger("CallTrigger");
        }
        else
        {
            //モーションが終了してアニメーターがIdle状態になれば行動終了
            IsComplete = true;
            return;
        }

        //仲間を呼ぶモーションを行ったことを記録
        if (stateInfo.IsName("Call")) IsCalled = true;
    }
}


