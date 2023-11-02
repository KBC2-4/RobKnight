using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Image;
using Color = System.Drawing.Color;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyBaseData;
    public EnemyData enemyData;

    [HideInInspector] public Animator animator;
    public EnemyBehavior behavior;

    private bool finded;
    private bool isAttacking = false;
    private bool playerFound = false;
    public bool isDeath = false;
    public Transform player;
    public float detectionRange = 1f;
    public LayerMask detectionLayer;    // Rayで検知するレイヤー
    //private LayerMask detectionLayer = LayerMask.GetMask("Player", "Wall");
    public float fieldOfViewAngle = 90f; // 視野角
    public int rayCount = 10; // 放射するRayの数
    
    public GameObject lightEffect;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
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
        enemyData.hp = enemyData.maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDeath)
        {
            DetectPlayer();
            behavior?.PerformActions(this);
        }
    }

    // void FixedUpdate()
    // {
    //     if (!isDeath)
    //     {
    //         behavior?.PerformActions(this);
    //     }
    // }

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
        lightEffect.SetActive(true);
        // Animator animator = lightEffect.GetComponent<Animator>();
        animator.SetBool("IsWalking", false);   
        Animation lightEffectAnimation = lightEffect.GetComponent<Animation>();
        if (lightEffectAnimation != null)
        {
            lightEffectAnimation.Play();
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }

    private IEnumerator DestroyAfterAnimation(string animationName, int layerIndex)
    {
        isDeath = true;

        // アニメーションの長さを取得
        float animationLength = animator.GetCurrentAnimatorStateInfo(layerIndex).length;

        // アニメーションが完了するのを待つ
        yield return new WaitForSeconds(animationLength);

        // ゲームオブジェクトを破壊
        //Destroy(gameObject);
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
        Debug.Log("エネミーが攻撃!");
    }
    public void EndAttack()
    {
        isAttacking = false;
        // ここでプレイヤーにダメージを与える処理を書く
        Debug.Log("エネミーが攻撃終了!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Player"))
        {
            // if (other.name == "AttackTrigger1")
            // {
            //     
            // }
            other.GetComponent<PlayerController>().Damage(enemyData.attackPower);
            //playerFound = true;
            EndAttack();
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerFound = false;
    //    }
    //}
    
    // プレイヤーを検出するための関数
    void DetectPlayer()
    {
        // プレイヤー検出フラグをリセット
        playerFound = false;
        
        // エネミーとプレイヤーの間の距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // プレイヤーが検出範囲内にいるかチェック
        if (distanceToPlayer <= detectionRange)
        {
            // エネミーからプレイヤーへの方向ベクトルを計算し、正規化
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            // エネミーの前方ベクトルとプレイヤーへの方向ベクトルの間の角度を計算
            float angleBetween = Vector3.Angle(transform.forward, directionToPlayer);

            // 角度が視野の半分より小さいかチェック
            if (angleBetween < fieldOfViewAngle / 2f)
            {
                
                // エネミーからプレイヤーへのレイを作成
                Ray ray = new Ray(transform.position, directionToPlayer);
                RaycastHit hit;
                // レイを赤色で描画（デバッグ用）
                Debug.DrawRay(ray.origin, ray.direction * detectionRange, UnityEngine.Color.red);

                // レイキャストを使用してプレイヤーを検出
                if (Physics.Raycast(ray, out hit, detectionRange, detectionLayer))
                {
                    // レイがプレイヤーにヒットしたかチェック
                    if (hit.collider.CompareTag("Player"))
                    {
                        // プレイヤーが見つからなかった時の処理
                        playerFound = true;
                        Debug.Log("エネミー：プレイヤーを見つけたよ！");
                    }
                    // else　// Raycastテスト
                    // {
                    //     // プレイヤーが見つからなかった時の処理
                    //     Debug.Log("エネミー：Raycastがヒットしたが、プレイヤーではない。ヒットしたオブジェクト：" + hit.collider.name);
                    //     playerFound = false;
                    // }
                }
            }

            // 放射線状のRayを描画
            for (int i = 0; i < rayCount; i++)
            {
                float angle = fieldOfViewAngle / (rayCount - 1) * i - fieldOfViewAngle / 2f;
                Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;
                Debug.DrawRay(transform.position, rayDirection * detectionRange, UnityEngine.Color.yellow);
            }
        }
    }

    public bool IsPlayerFound()
    {
        return playerFound;
    }

}
