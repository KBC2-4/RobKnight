using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector3 startPos;   // スポーン地点
    CharacterController controller;　// キャラクターコントローラー
    private Animator animator;
    public float speed = 5f;
    public float sensitivity = 30.0f;


    /// <summary>
    /// 入力用
    /// </summary>
    public InputActionAsset inputActions;

    /// <summary>
    /// 移動用入力変数
    /// </summary>
    private InputAction moveAction;

    /// <summary>
    /// 攻撃用入力変数
    /// </summary>
    private InputAction fireAction;

    /// <summary>
    /// 憑依用入力変数
    /// </summary>
    private InputAction possedAction;

    /// <summary>
    /// プレイヤーの移動量
    /// </summary>
    private Vector2 inputMove;

    /// <summary>
    /// プレイヤーの回転速度
    /// </summary>
    private float turnVelocity;

    /// <summary>
    /// 進行方向に向くのにかかる時間
    /// </summary>
    [SerializeField] private float smoothTime = 0.1f;

    float rotX, rotY;
    public EnemyData currentPossession; // 現在憑依しているエネミーのデータ
    private GameObject currentModel;

    public int hp = 100;
    public int maxHp = 100;
    public int mp = 100;
    public int attackPower = 10;
    private bool isAttacking = false;
    public ParticleSystem particleSystem;

    public static GameOverController Instance { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.enabled = true;
        animator.Play("Idle");
        particleSystem = GetComponentInChildren<ParticleSystem>();
        //particleSystem.Stop();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject!");
        }

        //inputActionsから[移動アクション]を取得
        moveAction = inputActions.FindActionMap("Player").FindAction("Move");
        moveAction.Enable();

        //inputActionsから[攻撃アクション]を取得
        fireAction = inputActions.FindActionMap("Player").FindAction("Fire");
        //[攻撃]アクションから呼ばれる関数を設定
        fireAction.performed += _ => AttackAnimation();
        fireAction.Enable();

        //inputActionsから[憑依アクション]を取得
        possedAction = inputActions.FindActionMap("Player").FindAction("Possession");
        possedAction.Enable();
 

        // マウスカーソルを非表示にする
        //Cursor.visible = false;
        //// マウスカーソルを画面の中央に固定する
        //Cursor.lockState = CursorLockMode.Locked;


        if (Application.isEditor)
        {
            sensitivity = sensitivity * 1.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false)
        {
            PlayerMove();
        }

        if(fireAction.IsPressed() == false)
        {
            isAttacking = false;
        }
    }

    void AttackAnimation()
    {
        isAttacking = true;
        Debug.Log("Attack");
        if (animator != null)
        {
            animator.SetTrigger("AttackTrigger");
        }
        if (particleSystem != null && particleSystem.isStopped)
        {
            particleSystem.Play();
        }
       
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Enemyかな？");
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            if (enemy != null)
            {
                //攻撃処理
                if (isAttacking)
                {
                    Debug.Log("Enemyだ！”攻撃開始");
                    enemy.Damage(attackPower);
                    isAttacking = false;
                }//憑依処理
                else if (possedAction.IsPressed() && enemy.enemyData.hp <= 0
                         && currentPossession == null)
                {
                    Debug.Log("prossession(憑依)");
                    Possession(other.gameObject);
                }
            }
        }
    }

    public void Damage(int damage)
    {
        //Debug.Log("エネミーから攻撃されています");
        hp -= damage;
        if (hp <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        animator.SetTrigger("DieTrigger");
        StartCoroutine(DestroyAfterAnimation("Die01_Stay_SwordAndShield", 0));
        Debug.Log("プレイヤーが死亡した！");
        GameOverController gameOverController = FindObjectOfType<GameOverController>();
        if (gameOverController != null)
        {
            gameOverController.ShowGameOverScreen();
        }
    }

    private IEnumerator DestroyAfterAnimation(string animationName, int layerIndex)
    {
        // アニメーションの長さを取得
        float animationLength = animator.GetCurrentAnimatorStateInfo(layerIndex).length;

        // アニメーションが完了するのを待つ
        yield return new WaitForSeconds(animationLength);
    }

    /// <summary>
    /// 移動用関数
    /// </summary>
    private void PlayerMove()
    {
        inputMove = moveAction.ReadValue<Vector2>();

        float speedX = inputMove.x * speed;
        float speedY = inputMove.y * speed;

        //入力値から、現在速度を計算
        Vector3 moveVelocity = new Vector3(speedX, 0, speedY);

        //現在フレームの移動量を移動速度から計算
        Vector3 moveDelta = moveVelocity * Time.deltaTime;

        //移動させる
        controller.Move(moveDelta);

        //キャラクターの回転
        if (inputMove != Vector2.zero)
        {
            animator.SetFloat("Speed", 1.0f);
            MoveRotation();
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

    }

    /// <summary>
    /// 移動時のキャラクターの回転関数
    /// </summary>
    private void MoveRotation()
    {
        //入力値からy軸周りの目標角度を計算
        var targetAngleY = -Mathf.Atan2(inputMove.y, inputMove.x) * Mathf.Rad2Deg + 90;
        //緩急させながら次の角度を計算
        var angleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnVelocity, smoothTime);
        //回転させる
        transform.rotation = Quaternion.Euler(0, angleY, 0);
    }

    /// <summary>
    /// 憑依アクション
    /// </summary>
    /// <param name="targetObj">憑依するキャラクター</param>
    private void Possession(GameObject targetObj)
    {
        //現在の体から操作機能を失効させる
        enabled = false;
        animator.enabled = false;

        //対象にプレイヤーコントローラーを追加
        targetObj.gameObject.AddComponent<CharacterController>();
        CharacterController characterController = targetObj?.gameObject.GetComponent<CharacterController>();
        // エネミーのCapsuleColliderからコリジョンの高さ・中心からの座標・半径を引継いでデストロイ
        CapsuleCollider capsuleCollider = targetObj?.gameObject.GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            characterController.height = capsuleCollider.height;
            characterController.center = capsuleCollider.center;
            characterController.radius = capsuleCollider.radius;
            Destroy(targetObj.GetComponent<CapsuleCollider>());
        }
        targetObj.gameObject.AddComponent<PlayerController>();

        PlayerController playerController = targetObj?.GetComponent<PlayerController>();

        if (playerController != null)
        {
            //操作対象を切り替える
            playerController.enabled = true;

            //憑依キャラのパラメータを設定
            currentPossession = targetObj?.GetComponent<EnemyController>().enemyData;
            playerController.maxHp = currentPossession.maxHp;
            playerController.hp = playerController.maxHp;
            playerController.attackPower = currentPossession.attackPower;
            playerController.inputActions = inputActions;
        }

        targetObj.GetComponent<EnemyController>().enabled = false;
            
        //タグをPlayerに変更
        targetObj.tag = "Player";

        //カメラのターゲットを憑依キャラに切り替える
        GameObject camera = GameObject.Find("MainCamera");
        if (camera != null)
        {
            camera.GetComponent<CameraMovement>().SetCameraTarget(targetObj);
        }
    }
}
