using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "EnemyBehavior")]
public class EnemyBehavior : ScriptableObject
{

    public bool UseAction = false;  //アクション配列に基づいて行動する? 
    private int ActionNum = 0;      //現在の行動配列番号(Action配列を使う場合のみ使用)
    private EnemyAction NowAction;  //現在の行動        (Action配列を使う場合のみ使用)

    public List<EnemyAction> actions;
    public EnemyAction attackAction;
    public EnemyAction idleAction;
    public List<EnemyAction> wanderAction;
    public EnemyAction chaseAction;
    public float idleFrequency = 0.1f; // アイドル状態になる確率を10%に設定
    public float idleTime = 2f; // アイドル状態の持続時間
    private bool isIdle = false;
    private float idleTimer = 0f; // アイドル用のタイマー
    public float attackRange = 2f; // 攻撃範囲
    private EnemyState currentState;
    private EnemyAction _selectedAction;
    private event Action _onDamageHandler;  // 攻撃受けたときに発生するイベントの経由イベント

    private void OnEnable()
    {
        //行動の初期化
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
        controller.transform.LookAt(controller.player); // プレイヤーの方向を向く
        attackAction.Act(controller); // 攻撃行動に移る
    }

    public void PerformActions(EnemyController controller)
    {
        // ダメージを受けたら即座に攻撃行動に移る
        // controller.OnDamage += () => attackAction.Act(controller);;
        // ダメージを受けたら即座に攻撃行動に移る
        // controller.OnDamage += () => {
        //     controller.transform.LookAt(controller.player); // プレイヤーの方向を向く
        //     attackAction.Act(controller); // 攻撃行動に移る
        // };


        //Debug.Log($"State:{currentState}");
        float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);

        //UseActionをチェックする
        if (UseAction)
        {
            //アクション配列に基づいて行動する
            actions[ActionNum].Act(controller);
            actions[ActionNum].ActionTime += Time.fixedDeltaTime;

            //Debug.Log($"State:{Time.fixedDeltaTime}");

            //行動終了時、新たな行動をセットする
            if (actions[ActionNum].IsComplete)
            {
                if (++ActionNum >= actions.Count) ActionNum = 0;

                actions[ActionNum].IsComplete = false;
                actions[ActionNum].ActionTime = 0;
            }

        }
        else
        {
            //アクション配列に基づかない行動
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
                    wanderAction[0].Act(controller);
                    currentState = EnemyState.Wander;

                    //if (_selectedAction == null)
                    //{
                    //    SelectAction(controller, wanderAction);
                    //}
                    //_selectedAction.Act(controller);
                    //currentState = EnemyState.Wander;

                    // ランダムな確率でアイドル状態に切り替える
                    if (Random.value < idleFrequency)
                    {
                        isIdle = true;
                    }
                }
            }
        }
       
    }
    
    
    // アクションをランダム選択
    private void SelectAction(EnemyController controller, List<EnemyAction> actions)
    {
        _selectedAction = actions[Random.Range(0, actions.Count)];
    }
}


