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
        if (!stateInfo.IsName("Call"))
        {
            if (!IsCalled)
            {
                controller.animator.SetTrigger("CallTrigger");
            }
            else
            {
                IsComplete = true;
            }
        }
        else 
        {
            IsCalled = true;
        }
    }
}


