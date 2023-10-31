using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyBaseData;
    public EnemyData enemyData;

    private Animator animator;
    public EnemyBehavior behavior;

    private bool finded;
    private bool isAttacking = false;
    private bool playerFound = false;
    public bool isDeath = false;
    public Transform player;
    public float detectionRange = 1f;
    public LayerMask detectionLayer;

    // Start is called before the first frame update
    void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    void Start()
    {
        if (enemyBaseData == null)
        {
            Debug.LogError("EnemyBaseDataがセットされていません。");
            return;
        }
        
        enemyData = Instantiate(enemyBaseData);
        animator = GetComponent<Animator>();
        enemyData.hp = enemyData.maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        behavior?.PerformActions(this);
    }

    public void Finded()
    {
        finded = true;
        animator.SetBool("Finded", true);
    }

    public void Damage(int damage)
    {
        Debug.Log("当たってます");
        enemyData.hp -= damage;
        if (enemyData.hp <= 0)
        {
            OnDeath();
        }
    }

    // エネミーが倒されたときの処理
    private void OnDeath()
    {
        animator.SetTrigger("DieTrigger");
        StartCoroutine(DestroyAfterAnimation("Die01", 0));
    }

    private IEnumerator DestroyAfterAnimation(string animationName, int layerIndex)
    {
        // アニメーションの長さを取得
        float animationLength = animator.GetCurrentAnimatorStateInfo(layerIndex).length;

        // アニメーションが完了するのを待つ
        yield return new WaitForSeconds(animationLength);

        // ゲームオブジェクトを破壊
        //Destroy(gameObject);
        isDeath = true;
    }

    public void OnMouseDown()
    {
        // ゲーム中にエネミーをクリックすると、プレイヤーが憑依する
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            //player.Possess(enemyData);
        }
    }

    //攻撃状況の取得
    public bool GetAttacking()
    {
        return isAttacking;
    }

    // アニメーションイベントから呼び出される関数
    public void PerformAttack()
    {
        isAttacking = true;
        // ここでプレイヤーにダメージを与える処理を書く
        //Debug.Log("エネミーが攻撃!");
    }
    public void EndAttack()
    {
        isAttacking = false;
        // ここでプレイヤーにダメージを与える処理を書く
        //Debug.Log("エネミーが攻撃終了!");
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerFound = true;
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerFound = false;
    //    }
    //}

    void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Ray ray = new Ray(transform.position, directionToPlayer);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, detectionRange, detectionLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    // プレイヤーが見つかった時の処理
                    playerFound = true;
                    Debug.Log("エネミー：プレイヤーを見つけたよ！");
                }
                else
                {
                    // プレイヤーが見つからなかった時の処理
                    playerFound = false;
                }
            }
        }
        else
        {
            // プレイヤーが見つからなかった時の処理
            playerFound = false;
        }
    }

    public bool IsPlayerFound()
    {
        return playerFound;
    }

}
