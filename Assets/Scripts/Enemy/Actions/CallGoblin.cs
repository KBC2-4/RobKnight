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
    private void OnEnable()
    {
        //�s���̏�����
        IsCalled = false;
    }

    public override void Act(EnemyController controller)
    {
        // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        Debug.Log($"State:{IsCalled}");

        //�A�N�V�����J�n�����莞�Ԃ��o���Ă��Ȃ��Ȃ�IsCalled�����Z�b�g
        if (ActionTime < 60) IsCalled = false;

        //���Ԃ��Ăԃ��[�V�������s�������Ƃ��L�^
        if (stateInfo.IsName("Call") && 60 <= ActionTime) IsCalled = true;

        if (!IsCalled)
        {
            //���Ԃ��Ăԃ��[�V�����̃g���K�[���Z�b�g
            controller.animator.SetTrigger("CallTrigger");
        }
        else if(stateInfo.IsName("Idle"))
        {
            //���[�V�������I�����ăA�j���[�^�[��Idle��ԂɂȂ�΍s���I��
            IsComplete = true;
            return;
        }
    }
}


