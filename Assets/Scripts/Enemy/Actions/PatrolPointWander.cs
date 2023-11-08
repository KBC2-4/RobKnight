using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/PatrolPointWanderAction")]
public class PatrolPointWander : EnemyAction
{
    public float moveSpeed = 2f;
    public Vector3[] patrolPoints;
    private int _nextPatrolPointIndex = 0;


    public override void Act(EnemyController controller)
    {
        Patrol(controller);

        float detectionRadius = 5.0f;
        float avoidanceStrength = 5.0f;
        
        // Collider[] hitColliders = Physics.OverlapSphere(controller.transform.position, detectionRadius);
        // foreach (var hitCollider in hitColliders)
        // {
        //     if (hitCollider.gameObject != controller.gameObject && !hitCollider.gameObject.CompareTag("Player")) // 自分自身を無視
        //     {
        //         Vector3 awayFromCollider = controller.transform.position - hitCollider.transform.position;
        //         controller.GetComponent<Rigidbody>().AddForce(awayFromCollider.normalized * avoidanceStrength);
        //     }
        // }
    }

    private void Patrol(EnemyController controller)
    {
        Rigidbody rb = controller.GetComponent<Rigidbody>();
        if (rb != null)
        {   
            // 現在再生中のアニメーションの状態を取得
            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
            controller.animator.SetFloat("Speed", moveSpeed);
            
            // 再生しているアニメーションにWalkタグが付いている場合
            if (stateInfo.IsTag("Move"))
            {
                // パトロールポイントが設定されているか確認
                if (patrolPoints != null && patrolPoints.Length > 0)
                {
                    //! Debug.Log("巡回開始！");
                    // 次のパトロールポイントへ移動
                    Vector3 nextPatrolPoint = patrolPoints[_nextPatrolPointIndex];
                    if (MoveTowardsPoint(controller, rb, nextPatrolPoint, moveSpeed))
                    {
                        //! Debug.Log("到着しました");
                        // パトロールポイントに到着したら次のポイントに移動
                        _nextPatrolPointIndex = (_nextPatrolPointIndex + 1) % patrolPoints.Length;
                    }
                }
            }
        }
    }

    // 指定されたポイントに移動するためのヘルパーメソッド
    private bool MoveTowardsPoint(EnemyController controller, Rigidbody rb, Vector3 targetPoint, float speed)
    {
        // targetPoint の Y 座標を現在の rb.position の Y 座標に設定することで無視
        targetPoint.y = rb.position.y;
        
        Vector3 direction = (targetPoint - rb.position).normalized;
        // エネミーがプレイヤーの方向を向く（Y軸の回転は除く）
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);
        
        float distanceToTarget = Vector3.Distance(rb.position, targetPoint);
        if (distanceToTarget > 0.1f)
        {
            Vector3 newPosition = rb.position + direction * (speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
            return false; // まだ目的地に到着していない
        }
        return true; // 目的地に到着した
    }
}
