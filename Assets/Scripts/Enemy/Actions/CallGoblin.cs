//-------------------------------
//  �{�X�S�u�����F���ԌĂ�
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
        // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        Debug.Log($"State:{IsCalled}");

        if (!IsCalled)
        {
            //���Ԃ��Ăԃ��[�V�����̃g���K�[���Z�b�g
            controller.animator.SetTrigger("CallTrigger");
        }
        else
        {
            //���[�V�������I�����ăA�j���[�^�[��Idle��ԂɂȂ�΍s���I��
            IsComplete = true;
            return;
        }

        //���Ԃ��Ăԃ��[�V�������s�������Ƃ��L�^
        if (stateInfo.IsName("Call")) IsCalled = true;
    }
}


