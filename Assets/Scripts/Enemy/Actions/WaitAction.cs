using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/WaitAction")]
public class WaitAction : EnemyAction
{
    public float FoundRange = 50;

    public override void Act(EnemyController controller)
    {
        //プレイヤー間の距離を取る
        float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);
        
        //プレイヤー間の距離が一定以下なら行動終了
        if (distanceToPlayer <= FoundRange)
        {
            IsComplete = true;
        }
    }
}


