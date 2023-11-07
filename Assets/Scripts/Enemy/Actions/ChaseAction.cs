using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/ChaseAction")]
public class ChaseAction : EnemyAction
{
    public float speed = 5f;

    public override void Act(EnemyController controller)
    {
        Chase(controller);
    }

    private void Chase(EnemyController controller)
    {
        
        Rigidbody rb = controller.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
            
            
            //�ړ����[�V�������Ȃ�G�l�~�[���ړ�����
            if (stateInfo.tagHash == Animator.StringToHash("Move"))
            {

                Vector3 direction = (controller.player.position - controller.transform.position).normalized;
                // �G�l�~�[���v���C���[�̕���������
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                // �X���[�Y�ɉ�]������
                controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);
                
                // �G�l�~�[���ړ�������
                Vector3 newPosition = rb.position + direction * (speed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
            }
            else
            {
                // �����A�j���[�V�������Đ�
                controller.animator.SetFloat("Speed", speed);
                
                // ���郂�[�V����������ꍇ�́A���郂�[�V�������g�p����
                if (controller && HasParameter(controller.animator,"RunTrigger"))
                {
                    controller.animator.SetTrigger("RunTrigger");
                }
            }
        }
    }
    public bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }
}