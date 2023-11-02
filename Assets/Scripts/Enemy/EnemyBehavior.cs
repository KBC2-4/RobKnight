using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyBehavior")]
public class EnemyBehavior : ScriptableObject
{
    public List<EnemyAction> actions;
    public EnemyAction attackAction;
    public EnemyAction idleAction;
    public EnemyAction wanderAction;
    public EnemyAction chaseAction;
    public float idleFrequency = 0.1f; // �A�C�h����ԂɂȂ�m����10%�ɐݒ�
    public float idleTime = 2f; // �A�C�h����Ԃ̎�������
    private bool isIdle = false;
    private float idleTimer = 0f; // �A�C�h���p�̃^�C�}�[
    public float attackRange = 2f; // �U���͈�
    private EnemyState currentState;

    private enum EnemyState
    {
        Idle,
        Wander,
        Chase,
        Attack
    }

    public void PerformActions(EnemyController controller)
    {
        Debug.Log($"State:{currentState}");
        float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);

        if (controller.IsPlayerFound())
        {
            if (distanceToPlayer <= attackRange)
            {
                attackAction.Act(controller);
                currentState = EnemyState.Attack;
                // foreach (var action in actions)
                // {
                //     action.Act(controller);
                // }
            }
            else
            {
                chaseAction.Act(controller);
                currentState = EnemyState.Chase;
            }
            
            // �v���C���[�������̓A�C�h���^�C�}�[�����Z�b�g
            idleTimer = 0f;
            isIdle = false;
        }
        else
        {
            if (isIdle)
            {
                if (idleTimer < idleTime)
                {
                    idleAction.Act(controller);
                    currentState = EnemyState.Idle;
                    idleTimer += Time.deltaTime; // �^�C�}�[���X�V
                }
                else
                {
                    // �A�C�h�����Ԃ��o�߂�����A��Ԃ�؂�ւ�
                    isIdle = false;
                    idleTimer = 0f;
                }
            }
            else
            {
                wanderAction.Act(controller);
                currentState = EnemyState.Wander;
                
                // �����_���Ȋm���ŃA�C�h����Ԃɐ؂�ւ���
                if (Random.value < idleFrequency)
                {
                    isIdle = true;
                }
            }
        }
    }
}


