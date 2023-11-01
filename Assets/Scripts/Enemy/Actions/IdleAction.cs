using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/IdleAction")]
public class IdleAction : EnemyAction
{
    public override void Act(EnemyController controller)
    {
        controller.animator.SetBool("IsWalking", false);
    }
}

