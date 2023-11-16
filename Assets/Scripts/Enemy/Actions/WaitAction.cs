using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/WaitAction")]
public class WaitAction : EnemyAction
{
    public float FoundRange = 50;

    public override void Act(EnemyController controller)
    {
        //ƒvƒŒƒCƒ„[ŠÔ‚Ì‹——£‚ğæ‚é
        float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);
        if (distanceToPlayer <= FoundRange)
        {
            IsComplete = true;
        }
    }
}


