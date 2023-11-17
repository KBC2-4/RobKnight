using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�Ƃ̋����ɂ���ċ������������߂Â����肵�Ȃ���U������

[CreateAssetMenu(menuName = "EnemyActions/MoveAndAttack")]
public class MoveAndAttack : EnemyAction
{
    public float attackRange = 2f;  //�U�����J�n����͈�

    public float BackSpeed = 1f;    //����鑬�x
    public float BackBorder = 1f;   //�����ړ����J�n���鋗��

    public float CloserSpeed = 1f;  //�ڋ߂��鑬�x
    public float CloserBorder = 4f; //�ڋ߂��J�n���鋗��

    public float AttackCool = 1f;   //�U���Ԋu
    private float NowCool = 1f;   //�U���Ԋu

    public override void Act(EnemyController controller)
    {
        
        // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        //�v���C���[�Ԃ̋��������
        float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);

        if (!stateInfo.IsName("Attack") && !stateInfo.IsTag("Attack"))
        {

            Vector3 direction = (controller.player.position - controller.transform.position).normalized;
            // �G�l�~�[���v���C���[�̕���������
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);

            if (distanceToPlayer <= attackRange && NowCool <= 0)      //�U�����J�n����
            {
                // �U���A�j���[�V�������Đ����łȂ�����AttackRange���Ƀv���C���[������ꍇ�̂݁A�g���K�[���Z�b�g
                controller.animator.SetTrigger("AttackTrigger");
            }
            else    //�ړ�����
            {

                CharacterController Cc = controller.GetComponent<CharacterController>();

                if (distanceToPlayer <= BackBorder) //�����ړ�
                {
                    if (Cc != null)
                    {
                        // �G�l�~�[���ړ�������
                        Vector3 newPosition = direction * (BackSpeed * Time.fixedDeltaTime);
                        if (stateInfo.IsName("Walk")) Cc.Move(-newPosition);
                        controller.animator.SetTrigger("WalkTrigger");
                        controller.animator.SetBool("IsEndWalk", false);
                        controller.animator.ResetTrigger("AttackTrigger");
                    }
                }
                else if (CloserBorder <= distanceToPlayer) //�ڋ߂���ړ�
                {
                    if (Cc != null)
                    {
                        // �G�l�~�[���ړ�������
                        Vector3 newPosition = direction * (CloserSpeed * Time.fixedDeltaTime);
                        if (stateInfo.IsName("Walk")) Cc.Move(newPosition);
                        controller.animator.SetTrigger("WalkTrigger");
                        controller.animator.SetBool("IsEndWalk", false);
                        controller.animator.ResetTrigger("AttackTrigger");
                    }
                }
                else 
                {
                    controller.animator.SetBool("IsEndWalk", true);
                }
            }

            if (5.0 <= ActionTime || IsComplete)
            {
                IsComplete = true;
            }
        }
        else
        {
            //�U���N�[���^�C�����Z�b�g
            NowCool = AttackCool;
        }

        NowCool -= Time.fixedDeltaTime;
        if (NowCool < 0) NowCool = 0;
    }
}


