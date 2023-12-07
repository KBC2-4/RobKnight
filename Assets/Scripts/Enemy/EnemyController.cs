using System;
using System.Collections;
using UnityEngine;
using Color = System.Drawing.Color;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyBaseData;
    public EnemyData enemyData;

    [HideInInspector] public Animator animator;
    public EnemyBehavior behavior;

    private bool _finded;

    private bool _isAttacking = false;  //攻撃中か否か
    private bool _isHit = false;        //当たり判定が残っているか？

    private bool _playerFound = false;
    public bool isDeath = false;
    public Transform player;
    public float detectionRange = 1f;
    public LayerMask detectionLayer;    // Rayで検知するレイヤー
    //private LayerMask detectionLayer = LayerMask.GetMask("Player", "Wall");
    public float fieldOfViewAngle = 90f; // 視野角
    public int rayCount = 10; // 放射するRayの数
    public GameObject uiPrompt; // 憑依を促すUI
    
    public GameObject lightEffect;
    private ParticleSystem _particleSystem; // 倒された時に表示するパーティクル

    // 攻撃受けた場合のイベント
    public event Action OnDamage;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        
        // プレイヤーが見つからない場合、再度検索する
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

        if (behavior != null)
        {
            // ビヘイビアの初期化(イベントハンドラの登録)
            behavior.Initialize(this);   
        }
        else
        {
            Debug.LogError("EnemyBehaviorがセットされていません。");
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDeath)
        {
            if (player != null)
            {
                // プレイヤーオブジェクトが非アクティブなら、再度検索する
                if (player.root.gameObject.activeSelf == false)
                {
                    player = GameObject.FindWithTag("Player").transform;
                }
                DetectPlayer();
                behavior?.PerformActions(this);
            }
            else
            {
                player = GameObject.FindWithTag("Player").transform;
            }
        }
    }

    // void FixedUpdate()
    // {
    //     if (!isDeath)
    //     {
    //         behavior?.PerformActions(this);
    //     }
    // }

    void OnDestroy()
    {
        if (behavior != null)
        {
            behavior.Cleanup(this);
        };
    }
    
    public void Finded()
    {
        _finded = true;
        animator.SetBool("Finded", true);
    }

    public void Damage(int damage)
    {
        //Debug.Log("当たってます");
        enemyData.hp -= damage;

        if (enemyData.hp <= 0)
        {
            OnDeath();
        }
        else
        {
            // ダメージイベントを発火
            OnDamage?.Invoke();
        }
    }

    // エネミーが倒されたときの処理
    private void OnDeath()
    {
        animator.SetTrigger("DieTrigger");
        StartCoroutine(DestroyAfterAnimation("Die01", 0));
        transform.rotation = Quaternion.identity;
        lightEffect.SetActive(true);
        // Animator animator = lightEffect.GetComponent<Animator>();
        animator.SetBool("IsWalking", false);
        animator.ResetTrigger("AttackTrigger");
        animator.ResetTrigger("RushTrigger");
        animator.ResetTrigger("CallTrigger");

        animator.SetBool("IsPossession", true);

        EndAttack();
        DisableHit();
        //Animation lightEffectAnimation = lightEffect.GetComponent<Animation>();
        //if (lightEffectAnimation != null)
        //{
        //    lightEffectAnimation.Play();
        //}
        // パーティクルを表示
        //_particleSystem = GetComponentInChildren<ParticleSystem>();

        //if (_particleSystem != null)
        //{
        //    _particleSystem.Play();
        //}
        //else
        //{
        //    Debug.Log("参照できませんでした。");
        //}

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        bool isPossession = player.gameObject.GetComponent<PlayerController>().isPossession;

        // ボスゴブリンまたはプレイヤーが憑依しているエネルギー以外は10秒後にオブジェクトを破棄する
        if (enemyData.enemyName != "BossGoblin" && isPossession)
        {
            // 10秒後にオブジェクトを破棄する。
            Destroy(gameObject, 10.0f);
        }
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
        return _isAttacking;
    }

    // アニメーションイベントから呼び出される関数
    public void PerformAttack()
    {
        _isAttacking = true;
        // ここでプレイヤーにダメージを与える処理を書く
        //Debug.Log("エネミーが攻撃!");
    }
    public void EndAttack()
    {
        _isAttacking = false;
        animator.ResetTrigger("AttackTrigger");
        // ここでプレイヤーにダメージを与える処理を書く
        //Debug.Log("エネミーが攻撃終了!");
    }

    //攻撃の当たり判定を有効化する
    public void EnableHit()
    {
        _isHit = true;
        // ここでプレイヤーにダメージを与える処理を書く
        //Debug.Log("エネミーが攻撃!");
    }

    //攻撃の当たり判定を無効化する
    public void DisableHit()
    {
        _isHit = false;
        // ここでプレイヤーにダメージを与える処理を書く
        //Debug.Log("エネミーが攻撃終了!");
    }

    void OnTriggerStay(Collider other)
    {
        if (_isHit && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Damage(enemyData.attackPower);
            //playerFound = true;
            _isHit = false;
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
       _playerFound = false;
        
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
                        _playerFound = true;
                        //Debug.Log("エネミー：プレイヤーを見つけたよ！");
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
        return _playerFound;
    }

    //ステータス取得
    public int IsGetPower() 
    {
        return enemyData.attackPower;
    }

}
