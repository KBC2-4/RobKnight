using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/CallGoblin")]
public class CallGoblin : EnemyAction
{
    private bool IsCalled = false;

    public override void Act(EnemyController controller)
    {
        // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
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


