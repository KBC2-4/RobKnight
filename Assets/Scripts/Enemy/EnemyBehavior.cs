using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyBehavior")]
public class EnemyBehavior : ScriptableObject
{
    public List<EnemyAction> actions;
    public EnemyAction idleAction;
    public EnemyAction wanderAction;
    public float idleFrequency = 0.1f;
    private bool isIdle = false;

    public void PerformActions(EnemyController controller)
    {

        //if (controller.IsPlayerFound())
        //{
        //    Debug.Log("プレイヤーを見つけた！");
        //    foreach (var action in actions)
        //    {
        //        action.Act(controller);
        //    }
        //}
        //else if (Random.value < idleFrequency)
        //{
        //    Debug.Log("プレイヤー探索中！");
        //    idleAction.Act(controller);
        //}
        //else
        //{
        //    wanderAction.Act(controller);
        //}

        if (controller.IsPlayerFound())
        {
            foreach (var action in actions)
            {
                action.Act(controller);
            }
        }
        else
        {
            if (isIdle)
            {
                idleAction.Act(controller);
            }
            else
            {
                wanderAction.Act(controller);
            }

            if (Random.value < idleFrequency)
            {
                isIdle = !isIdle;
            }
        }
    }
}


