// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class EnemyWeapon : MonoBehaviour
// {
//
//     private bool isAttacking = false;
//     private bool beforeAttack = false;
//     public int attackDamage = 0;
//     private GameObject rootobj = null;
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         //親オブジェクトを取得
//         rootobj = transform.root.gameObject;
//
//         //攻撃力設定
//         if (rootobj.CompareTag("Enemy"))
//         {
//             EnemyAttack enemyAttack = rootobj.GetComponent<EnemyAttack>();
//             attackDamage = enemyAttack.attackDamage;
//         }
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         if ( rootobj.CompareTag("Enemy"))
//         {
//             EnemyAttack enemyAttack = rootobj.GetComponent<EnemyAttack>();
//             if (enemyAttack.GetAttacking() && beforeAttack != enemyAttack.GetAttacking()) 
//             {
//                 isAttacking = true;
//             }
//
//             beforeAttack = enemyAttack.GetAttacking();
//         }
//     }
//
//     // 当たり判定がプレイヤーに触れたときの処理
//     private void OnTriggerStay(Collider other)
//     {
//         if (isAttacking && other.CompareTag("Player"))
//         {
//             PlayerController playerMovement = other.GetComponent<PlayerController>();
//             if (playerMovement != null)
//             {
//                 playerMovement.Damage(attackDamage);
//                 isAttacking = false;
//             }
//         }
//     }
// }
