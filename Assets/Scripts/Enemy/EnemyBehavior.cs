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
    public float idleFrequency = 0.1f; // アイドル状態になる確率を10%に設定
    public float idleTime = 2f; // アイドル状態の持続時間
    private bool isIdle = false;
    private float idleTimer = 0f; // アイドル用のタイマー
    public float attackRange = 2f; // 攻撃範囲
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
            
            // プレイヤー発見時はアイドルタイマーをリセット
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
                    idleTimer += Time.deltaTime; // タイマーを更新
                }
                else
                {
                    // アイドル時間が経過したら、状態を切り替え
                    isIdle = false;
                    idleTimer = 0f;
                }
            }
            else
            {
                wanderAction.Act(controller);
                currentState = EnemyState.Wander;
                
                // ランダムな確率でアイドル状態に切り替える
                if (Random.value < idleFrequency)
                {
                    isIdle = true;
                }
            }
        }
    }
}


