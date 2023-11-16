//-------------------------------
//  �{�X�S�u�����F���ԌĂ�
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/CallGoblin")]
public class CallGoblin : EnemyAction
{

    public override void Act(EnemyController controller)
    {
        // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("EndCall")) 
        {
            //���[�V�������I�����ăA�j���[�^�[��Idle��ԂɂȂ�΍s���I��
            IsComplete = true;
            controller.animator.ResetTrigger("CallTrigger");
            return;
        }
        else if (stateInfo.IsName("Idle") && !IsComplete)
        {
            //���Ԃ��Ăԃ��[�V�����̃g���K�[���Z�b�g
            controller.animator.SetTrigger("CallTrigger");
        }
    }
}


