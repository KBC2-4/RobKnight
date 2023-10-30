using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/AttackAction")]
public class AttackAction : EnemyAction
{
    public float attackRange = 2f;

    public override void Act(EnemyController controller)
    {
        //float distanceToPlayer = Vector3.Distance(controller.transform.position, PlayerController.PlayerInstance.transform.position);

        //if (distanceToPlayer <= attackRange && !controller.GetAttacking())
        //{
        controller.GetComponent<Animator>().SetBool("IsWalking", false);
        controller.GetComponent<Animator>().SetTrigger("AttackTrigger");
        //}
    }
}


