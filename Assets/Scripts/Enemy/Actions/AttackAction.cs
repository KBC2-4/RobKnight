using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/AttackAction")]
public class AttackAction : EnemyAction
{
    public float attackRange = 2f;  //�U���͈�
    public float attackCool = 0;    //�U���Ԋu
    private float nowCool;

    public override void Act(EnemyController controller)
    {
        
        // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
        
            controller.animator.SetFloat("Speed", 0);
        if (!stateInfo.IsName("Attack") && !stateInfo.IsTag("Attack"))
        {

            Vector3 direction = (controller.player.position - controller.transform.position).normalized;
            // �G�l�~�[���v���C���[�̕���������
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);

            //�v���C���[�Ԃ̋��������
            float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);
            if (distanceToPlayer <= attackRange && nowCool <= 0)
            {
                // �U���A�j���[�V�������Đ����łȂ�����AttackRange���Ƀv���C���[������ꍇ�̂݁A�g���K�[���Z�b�g
                controller.animator.SetTrigger("AttackTrigger");
            }

            if (5.0 <= ActionTime || IsComplete)
            {
                IsComplete = true;
            }
        }
        else 
        {
            nowCool = attackCool;
        }

        nowCool -= Time.deltaTime;
        if (nowCool < 0) nowCool = 0;
    }
}


