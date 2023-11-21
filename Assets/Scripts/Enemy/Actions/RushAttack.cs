//-------------------------------
//  �{�X�S�u�����F���ԌĂ�
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/RushAttack")]
public class RushAttack : EnemyAction
{
    public int RushTime = 60;    //�ːi�̌p������
    public float RushSpeed = 5;  //�ːi�̏������x
    public float RushRange = 3;  //�ːi�̍U���͈�
    public int HitCool = 20;     //�ːi�̓����蔻��N�[���^�C��

    private float NowSpeed = 5;  //�ːi�̑��x
    private Vector3 direction;  //�ːi�̊p�x
    private int StartTime;  //�ːi�̊J�n����
    private int NowCool;    //���݂̃N�[���^�C��

    public override void Act(EnemyController controller)
    {
        // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        //�ːi�̊J�n�ƏI��
        if (stateInfo.IsName("StartRush"))  //�ːi�J�n��
        {
            StartTime = ActionTime; //�ːi�̊J�n���Ԃ�����
            NowSpeed = 0;   //�ːi�̑��x��������

            direction = (controller.player.position - controller.transform.position).normalized;
            // �ːi�̕��������߂�
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            // �X���[�Y�ɉ�]������
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else if (StartTime + RushTime < ActionTime) //�ːi�J�n����RushTime�Ԃ�̎��Ԃ��o�߂�����
        {
            //�ːi�I�����[�V�����̃g���K�[���Z�b�g
            controller.animator.SetTrigger("RushEndTrigger");
        }

        //�ːi�ɔ����ړ�
        if (stateInfo.IsName("Rush") || stateInfo.IsName("EndRush")) 
        {
            Rigidbody rb = controller.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // �G�l�~�[���ړ�������
                Vector3 newPosition = rb.position + direction * (NowSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
            }

            //�ːi���̍s��
            if (stateInfo.IsName("Rush")) 
            {
                //�ːi�J�n���A�ړ����x�����X�ɏグ��
                NowSpeed += RushSpeed / (RushTime * 0.6f);
                if (RushSpeed < NowSpeed) NowSpeed = RushSpeed;

                //�v���C���[�Ԃ̋��������
                float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);
                //�v���C���[�̋��������ȉ��Ȃ�ːi�_���[�W��^����
                if (distanceToPlayer <= RushRange && NowCool <= 0) 
                {
                    controller.player.GetComponent<PlayerController>().Damage(controller.IsGetPower());
                    NowCool = HitCool;
                }
            }
            //�ːi�I�����̍s��
            else
            {
                //�ːi�I�����A�ړ����x�����X�Ɍ��炷
                NowSpeed -= RushSpeed / 30;
                if (NowSpeed < 0) NowSpeed = 0;
            }
        }

        //�ːi�N�[���^�C��������������
        if (--NowCool < 0) NowCool = 0;

        //�A�j���[�V�����̏�ԑJ��
        if (stateInfo.IsName("EndRush"))
        {
            if (NowSpeed <= 0)
            {
                //�ːi���[�V�������I����A���x��0�ȉ��Ȃ�ːi�A�N�V�������I������
                IsComplete = true;
                controller.animator.ResetTrigger("RushTrigger");
                controller.animator.ResetTrigger("RushEndTrigger");
                return;
            }
        }
        else if (stateInfo.IsName("Idle") && !IsComplete)
        {
            //�ːi���[�V�����̃g���K�[���Z�b�g
            controller.animator.SetTrigger("RushTrigger");
        }

    }
}


