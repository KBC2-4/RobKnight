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
        //float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);
        // if (distanceToPlayer <= attackRange)
        // {
        //     Debug.Log("�G�l�~�[�F�U�����[�V����");
        //     controller.GetComponent<Animator>().SetBool("IsWalking", false);
        //     controller.GetComponent<Animator>().SetTrigger("AttackTrigger");
        // }
        
        // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

            controller.animator.SetBool("IsWalking", false);
        if (!stateInfo.IsName("Attack"))
        {

            Vector3 direction = (controller.player.position - controller.transform.position).normalized;
            // �G�l�~�[���v���C���[�̕���������
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);

            // �U���A�j���[�V�������Đ����łȂ��ꍇ�̂݁A�g���K�[���Z�b�g
            controller.animator.SetTrigger("AttackTrigger");
        }
    }
}


