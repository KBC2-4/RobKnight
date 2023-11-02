using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/WanderAction")]
public class WanderAction : EnemyAction
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 50f;

    public override void Act(EnemyController controller)
    {
        // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
        controller.animator.SetBool("IsWalking", true);

        //���S���Ă��Ȃ� & �A�j���[�^�[�̃X�e�[�g��Move01�Ȃ�ړ������ɓ���
        if (!controller.isDeath && stateInfo.IsName("Move01"))
        {
            controller.transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
            controller.transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));
            // !
        }
    }
}

