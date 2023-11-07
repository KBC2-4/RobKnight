using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/PatrolPointWanderAction")]
public class PatrolPointWander : EnemyAction
{
    public float moveSpeed = 2f;
    public Vector3[] patrolPoints;
    private int nextPatrolPointIndex = 0;

    public override void Act(EnemyController controller)
    {
        Patrol(controller);
    }

    private void Patrol(EnemyController controller)
    {
        Rigidbody rb = controller.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
            controller.animator.SetFloat("Speed", moveSpeed);
            
            // �Đ����Ă���A�j���[�V������Walk�^�O���t���Ă���ꍇ
            if (stateInfo.IsTag("Walk"))
            {
                // �p�g���[���|�C���g���ݒ肳��Ă��邩�m�F
                if (patrolPoints != null && patrolPoints.Length > 0)
                {
                    // ���̃p�g���[���|�C���g�ֈړ�
                    Vector3 nextPatrolPoint = patrolPoints[nextPatrolPointIndex];
                    if (MoveTowardsPoint(controller, rb, nextPatrolPoint, moveSpeed))
                    {
                        // �p�g���[���|�C���g�ɓ��������玟�̃|�C���g�Ɉړ�
                        nextPatrolPointIndex = (nextPatrolPointIndex + 1) % patrolPoints.Length;
                    }
                }
            }
        }
    }

    // �w�肳�ꂽ�|�C���g�Ɉړ����邽�߂̃w���p�[���\�b�h
    private bool MoveTowardsPoint(EnemyController controller, Rigidbody rb, Vector3 targetPoint, float speed)
    {
        Vector3 direction = (targetPoint - rb.position).normalized;
        // �G�l�~�[���v���C���[�̕���������
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);
        
        float distanceToTarget = Vector3.Distance(rb.position, targetPoint);
        if (distanceToTarget > 0.1f)
        {
            Vector3 newPosition = rb.position + direction * (speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
            return false; // �܂��ړI�n�ɓ������Ă��Ȃ�
        }
        return true; // �ړI�n�ɓ�������
    }
}
