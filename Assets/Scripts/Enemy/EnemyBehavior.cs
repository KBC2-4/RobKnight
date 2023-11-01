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
    public float idleFrequency = 0.1f;
    private bool isIdle = false;
    public float attackRange = 2f; // çUåÇîÕàÕ

    private float timer;
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
        }
        else
        {
            if (isIdle)
            {
                idleAction.Act(controller);
                currentState = EnemyState.Idle;
            }
            else
            {
                wanderAction.Act(controller);
                currentState = EnemyState.Wander;
            }

            if (Random.value < idleFrequency)
            {
                isIdle = !isIdle;
            }
        }
    }
}


