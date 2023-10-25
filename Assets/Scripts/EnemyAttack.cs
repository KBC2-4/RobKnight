using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 10;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // アニメーションイベントから呼び出される関数
    public void PerformAttack()
    {
        isAttacking = true;
        // ここでプレイヤーにダメージを与える処理を書く
        Debug.Log("エネミーが攻撃!");
    }

    // 当たり判定がプレイヤーに触れたときの処理
    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Damage(attackDamage);
                isAttacking = false;
            }
        }
    }
}
