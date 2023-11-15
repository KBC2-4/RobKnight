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
    private void OnEnable()
    {
        //行動の初期化
        IsCalled = false;
    }

    public override void Act(EnemyController controller)
    {
        // 現在再生中のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        Debug.Log($"State:{IsCalled}");

        //アクション開始から一定時間が経っていないならIsCalledをリセット
        if (ActionTime < 60) IsCalled = false;

        //仲間を呼ぶモーションを行ったことを記録
        if (stateInfo.IsName("Call") && 60 <= ActionTime) IsCalled = true;

        if (!IsCalled)
        {
            //仲間を呼ぶモーションのトリガーをセット
            controller.animator.SetTrigger("CallTrigger");
        }
        else if(stateInfo.IsName("Idle"))
        {
            //モーションが終了してアニメーターがIdle状態になれば行動終了
            IsComplete = true;
            return;
        }
    }
}


