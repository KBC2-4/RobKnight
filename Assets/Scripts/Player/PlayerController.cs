using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    private InputAction possessionAction;

    /// <summary>
    /// 人間に戻る用入力変数
    /// </summary>
    private InputAction returnAction;

    /// <summary>
    /// 人間に戻ったときの座標
    /// </summary>
    private Vector3 returnPosition = new Vector3(300f, 1f, 400f);

    /// <summary>
    /// 憑依アクション入力タイマー
    /// </summary>
    private float inputTimerPossession;

    /// <summary>
    /// 憑依アクションに必要な入力時間
    /// </summary>
    public const float inputTimePossession = 0.3f;

    /// <summary>
    /// 憑依後に保存するための変数
    /// </summary>
    private GameObject player = null;

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
    private EnemyData currentPossession; // 現在憑依しているエネミーのデータ
    private GameObject currentModel;

    private int hp = 250;
    private int maxHp = 250;
    public int mp = 100;
    public int attackPower = 10;
    private bool isAttacking = false;
    public ParticleSystem particleSystem;

    public static GameOverController Instance { get; private set; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    // 憑依しているか
    public bool isPossession = false;
    // 憑依しているエネミーの名前を取得
    public string PossessionEnemyName;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        returnPosition = transform.position;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.enabled = true;
        animator.Play("Idle");
        particleSystem = GetComponentInChildren<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }

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
        possessionAction = inputActions.FindActionMap("Player").FindAction("Possession");
        possessionAction.Enable();

        //inputActionsから[人間に戻るアクション]を取得
        returnAction = inputActions.FindActionMap("Player").FindAction("Return");
        returnAction.performed += _ => Return();
        returnAction.Enable();

        currentPossession = null;

        // マウスカーソルを非表示にする
        //Cursor.visible = false;
        //// マウスカーソルを画面の中央に固定する
        //Cursor.lockState = CursorLockMode.Locked;



        if (Application.isEditor)
        {
            sensitivity = sensitivity * 1.5f;
        }
    }

    private void OnEnable()
    {
        inputTimerPossession = 0;
        isPossession = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        //Debug.Log($"State:{isAttacking}");

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            isAttacking = false;
        }

        //Debug.Log($"State:{transform.position}");
        //Debug.Log($"State:{GetComponent<Rigidbody>().position}");
    }

    void AttackAnimation()
    {
        // 現在再生中のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (animator != null && !isAttacking)
        {
            //animator.Play("Attack");
            animator.SetTrigger("AttackTrigger");
        }
        if (particleSystem != null && particleSystem.isStopped)
        {
            particleSystem.Play();
        }
    }

    // アニメーションイベントから呼び出される関数
    public void PerformAttack()
    {
        isAttacking = true;
    }
    public void EndAttack()
    {
        isAttacking = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            if (enemy != null)
            {
                if (possessionAction != null)
                {
                    if (possessionAction.IsPressed() == true)
                    {
                        Debug.Log("posTime" + inputTimerPossession);
                        inputTimerPossession += Time.deltaTime;

                        if (inputTimePossession < inputTimerPossession
                            && enemy.enemyData.hp <= 0 && isPossession == false)
                        {
                            Possession(other.gameObject);
                        }
                    }
                    else
                    {
                        inputTimerPossession = 0;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            if (enemy != null)
            {
                //攻撃
                if (isAttacking == true && 0 < enemy.enemyData.hp)
                {
                    enemy.Damage(attackPower);
                }
            }
        }
    }

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            //憑依状態であれば人間体に戻す
            if (name != "Player")
            {
                Debug.Log("Damage:return");
                Return();
            }
            else
            {
                Debug.Log("Damage:deth");
                OnDeath();
            }
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
    private Vector3 force = new Vector3(0, 0, 0);   //押し出す力
    private Vector3 forcedecay = new Vector3(0, 0, 0);   //押し出す力の減衰
    private float forcetime = 0;                      //押し出す時間
    private void PlayerMove()
    {
        inputMove = moveAction.ReadValue<Vector2>();

        float speedX = inputMove.x * speed;
        float speedY = inputMove.y * speed;

        //入力値から、現在速度を計算
        Vector3 moveVelocity = new Vector3(speedX, 0, speedY);

        //現在フレームの移動量を移動速度から計算
        Vector3 moveDelta = moveVelocity * Time.deltaTime;


        if ((inputMove != Vector2.zero || 0 < forcetime)
            && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") == false)
        {
            //移動させる
            controller.Move(moveDelta);

            //押し出す力を移動に加える
            if (0 < forcetime) controller.Move(force);

            if (player != null)
            {
                player.transform.position = transform.position;
            }

            animator.SetFloat("Speed", 1.0f);
            //キャラクターの回転
            MoveRotation();
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        //押し出す時間と力を減らす
        forcetime -= Time.fixedDeltaTime;
        if (forcetime < 0)
        {
            force = Vector3.zero;
            forcedecay = Vector3.zero;
            forcetime = 0;
        }
        else 
        {
            force -= (forcedecay * Time.fixedDeltaTime);
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
        if(player != null) 
        {
            player.transform.rotation= Quaternion.Euler(0, angleY, 0);
        }
    }

    //プレイヤーがノックバックする force:押し出す力 Base:ノックバックの発生地点
    public void KnockBack(float force, float time, Vector3 Base)
    {
        Vector3 PlayerPos = transform.position;

        PlayerPos.y = 0;
        Base.y = 0;

        //プレイヤーと対象間の角度を取る
        var diff = (PlayerPos - Base).normalized;
        Vector3 PushAngle = diff * (force * time);

        this.force = PushAngle;
        forcedecay = PushAngle / time;
        forcetime = time;
    }

    /// <summary>
    /// 憑依アクション
    /// </summary>
    /// <param name="targetObj">憑依するキャラクター</param>
    private void Possession(GameObject targetObj)
    {
        //"Player"(人間)であれば非表示にする
        if (name == "Player")
        {
            inputTimerPossession = 0f;
            player = gameObject;
            gameObject.SetActive(false);
        }//憑依体であれば破棄する
        else
        {
           
            Destroy(gameObject);
        }
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
            //Destroy(targetObj.GetComponent<CapsuleCollider>());
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
            playerController.player = player;
            playerController.PossessionEnemyName = currentPossession.enemyName;
            player = null;
            playerController.currentPossession = currentPossession;
            currentPossession = null;

            //PlayerのHPsliderを憑依体のHPに設定
            PlayerHpSlider playerHpSlider = GameObject.Find("HP").GetComponent<PlayerHpSlider>();
            if (playerHpSlider != null)
            {
                playerHpSlider.SetPlayerHp(playerController);
                playerHpSlider.hpSlider.fillRect.GetComponent<Image>().color = new Color(0.8030842f, 0.4134211f, 0.9245283f, 1.0f);
            }

        }

        EnemyController enemyController = targetObj?.GetComponent<EnemyController>();
        if(enemyController != null) 
        {
            //敵のアニメーターステータスを変更
            enemyController.animator.SetBool("IsPossession", true);

            //ライトエフェクトを削除
            enemyController.lightEffect.SetActive(false);
            enemyController.enemyData.hp = playerController.maxHp;
            Destroy(enemyController);
        }

        //タグをPlayerに変更
        targetObj.tag = "Player";
        targetObj.layer = gameObject.layer;

        //憑依した敵のHPバーを削除
        Canvas canvas=targetObj?.GetComponentInChildren<Canvas>();
        if(canvas != null) 
        {
            canvas.enabled = false;
        }
        
        //カメラのターゲットを憑依キャラに切り替える
        GameObject camera = GameObject.Find("MainCamera");
        if (camera != null)
        {
            camera.GetComponent<CameraMovement>().SetCameraTarget(targetObj);
        }

        isPossession = true;
    }

    /// <summary>
    /// 人間に戻るアクション
    /// </summary>
    private void Return()
    {
        if (player != null)
        {
            //"Player"(人間)を表示する
            player.SetActive(true);

            //PlayerのHPsliderを元に戻す
            PlayerHpSlider playerHpSlider = GameObject.Find("HP").GetComponent<PlayerHpSlider>();
            if (playerHpSlider != null)
            {
                playerHpSlider.SetPlayerHp(player.GetComponent<PlayerController>());
                playerHpSlider.hpSlider.fillRect.GetComponent<Image>().color = new Color(0.6705883f, 1.0f, 0.5803922f, 1.0f);
            }

            //カメラのターゲットを"Player"(人間)に戻す
            GameObject camera = GameObject.Find("MainCamera");
            if (camera != null)
            {
               camera.GetComponent<CameraMovement>().SetCameraTarget(player);
            }

            //憑依体を削除
            player = null;
            Destroy(gameObject);
        }
    }

    public EnemyData GetPossessionData()
    {
        return currentPossession;
    }

    public int GetPlayerMaxHp()
    {
        return maxHp;
    }

    public int GetPlayerHp() 
    {
        return hp;
    }
}   
