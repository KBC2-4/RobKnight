using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/WanderAction")]
public class WanderAction : EnemyAction
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 50f;

    public override void Act(EnemyController controller)
    {
        controller.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        controller.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        // !
        //controller.GetComponent<Animator>().SetBool("IsWalking", true);

    }
}

