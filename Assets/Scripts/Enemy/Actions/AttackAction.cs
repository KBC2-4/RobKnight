using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/AttackAction")]
public class AttackAction : EnemyAction
{
    public float attackRange = 2f;

    public override void Act(EnemyController controller)
    {
        // if (controller == null || controller.player == null)
        // {
        //     Debug.LogError("Controller�܂���Player��null�ł�!");
        //     return;
        // }
        //
        // float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);
        // if (distanceToPlayer <= attackRange)
        // {
        //     Debug.Log("�G�l�~�[�F�U�����[�V����");
        //     controller.GetComponent<Animator>().SetBool("IsWalking", false);
        //     controller.GetComponent<Animator>().SetTrigger("AttackTrigger");
        // }
        
        controller.animator.SetBool("IsWalking", false);
        // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName("Attack"))
        {
            // �U���A�j���[�V�������Đ����łȂ��ꍇ�̂݁A�g���K�[���Z�b�g
            controller.animator.SetTrigger("AttackTrigger");
        }
    }
}


