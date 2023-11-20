using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "EnemyBehavior")]
public class EnemyBehavior : ScriptableObject
{

    public bool UseAction = false;  //�A�N�V�����z��Ɋ�Â��čs������? 
    private int ActionNum = 0;      //���݂̍s���z��ԍ�(Action�z����g���ꍇ�̂ݎg�p)
    private EnemyAction NowAction;  //���݂̍s��        (Action�z����g���ꍇ�̂ݎg�p)

    public List<EnemyAction> actions;
    public EnemyAction attackAction;
    public EnemyAction idleAction;
    public List<EnemyAction> wanderAction;
    public EnemyAction chaseAction;
    public float idleFrequency = 0.1f; // �A�C�h����ԂɂȂ�m����10%�ɐݒ�
    public float idleTime = 2f; // �A�C�h����Ԃ̎�������
    private bool isIdle = false;
    private float idleTimer = 0f; // �A�C�h���p�̃^�C�}�[
    public float attackRange = 2f; // �U���͈�
    private EnemyState currentState;
    private EnemyAction _selectedAction;
    private event Action _onDamageHandler;  // �U���󂯂��Ƃ��ɔ�������C�x���g�̌o�R�C�x���g

    private void OnEnable()
    {
        //�s���̏�����
        ActionNum = 0;
        for (int i = 0; i < actions.Count; i++) 
        {
            actions[ActionNum].ActionTime = 0;
            actions[ActionNum].IsComplete = false;
        }
    }
    
    public void Initialize(EnemyController controller)
    {
        _onDamageHandler = () => TriggerCounterAttack(controller);
        controller.OnDamage += _onDamageHandler;
    }
    
    public void Cleanup(EnemyController controller)
    {
        controller.OnDamage -= _onDamageHandler;
    }

    private enum EnemyState
    {
        Idle,
        Wander,
        Chase,
        Attack
    }

    private void TriggerCounterAttack(EnemyController controller)
    {
        controller.transform.LookAt(controller.player); // �v���C���[�̕���������
        attackAction.Act(controller); // �U���s���Ɉڂ�
    }

    public void PerformActions(EnemyController controller)
    {
        // �_���[�W���󂯂��瑦���ɍU���s���Ɉڂ�
        // controller.OnDamage += () => attackAction.Act(controller);;
        // �_���[�W���󂯂��瑦���ɍU���s���Ɉڂ�
        // controller.OnDamage += () => {
        //     controller.transform.LookAt(controller.player); // �v���C���[�̕���������
        //     attackAction.Act(controller); // �U���s���Ɉڂ�
        // };


        //Debug.Log($"State:{currentState}");
        float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);

        //UseAction���`�F�b�N����
        if (UseAction)
        {
            //�A�N�V�����z��Ɋ�Â��čs������
            actions[ActionNum].Act(controller);
            actions[ActionNum].ActionTime += Time.fixedDeltaTime;

            //Debug.Log($"State:{Time.fixedDeltaTime}");

            //�s���I�����A�V���ȍs�����Z�b�g����
            if (actions[ActionNum].IsComplete)
            {
                if (++ActionNum >= actions.Count) ActionNum = 0;

                actions[ActionNum].IsComplete = false;
                actions[ActionNum].ActionTime = 0;
            }

        }
        else
        {
            //�A�N�V�����z��Ɋ�Â��Ȃ��s��
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
                    wanderAction[0].Act(controller);
                    currentState = EnemyState.Wander;

                    //if (_selectedAction == null)
                    //{
                    //    SelectAction(controller, wanderAction);
                    //}
                    //_selectedAction.Act(controller);
                    //currentState = EnemyState.Wander;

                    // �����_���Ȋm���ŃA�C�h����Ԃɐ؂�ւ���
                    if (Random.value < idleFrequency)
                    {
                        isIdle = true;
                    }
                }
            }
        }
       
    }
    
    
    // �A�N�V�����������_���I��
    private void SelectAction(EnemyController controller, List<EnemyAction> actions)
    {
        _selectedAction = actions[Random.Range(0, actions.Count)];
    }
}


