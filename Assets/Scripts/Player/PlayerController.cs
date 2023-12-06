using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    private InputControls inputActions;

    private HashSet<int> _hitEnemyList;  //攻撃に触れたエネミーリスト

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

    private EnemyData currentPossession; // 現在憑依しているエネミーのデータ

    private int hp = 250;
    private int maxHp = 250;
    public int attackPower = 10;
    private bool isAttacking = false;
    public ParticleSystem slashEffect;
    private PlayerHpSlider _hpSlider;

    public static GameOverController Instance { get; private set; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    // 憑依しているか
    public bool isPossession = false;
    private bool canPossesion = false;
    // 憑依しているエネミーの名前を取得
    public string PossessionEnemyName;

    private void Awake()
    {
        inputActions = new InputControls();

        inputActions.Enable();

        _hitEnemyList = new HashSet<int>();


        _hpSlider = GameObject.Find("HP").GetComponent<PlayerHpSlider>();

        
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.Play("Idle");
        slashEffect = GetComponentInChildren<ParticleSystem>();
        if(slashEffect != null) 
        {
            slashEffect.Stop();
        }
        
        //particleSystem.Stop();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject!");
        }

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
        inputActions.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Fire.performed += AttackAnimation;
        inputActions.Player.Possession.performed += CanPossesion;
        inputActions.Player.Possession.canceled += CanPossesion;
        inputActions.Player.Return.performed += ReturnAction;

        isPossession = false;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnDestroy()
    {
        //inputActinosのコールバックの解除
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Fire.performed -= AttackAnimation;
        inputActions.Player.Possession.performed -= CanPossesion;
        inputActions.Player.Possession.canceled -= CanPossesion;
        inputActions.Player.Return.performed -= ReturnAction;

        //入力コントローラーの削除
        inputActions.Dispose();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        PlayerMove();

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            isAttacking = false;
        }

        //Debug.Log($"State:{transform.position}");
        //Debug.Log($"State:{GetComponent<Rigidbody>().position}");
    }

    void AttackAnimation(InputAction.CallbackContext context)
    {
        if (context.performed == true && Time.timeScale != 0)
        {
            // 現在再生中のアニメーションの状態を取得
            //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (animator != null && !isAttacking)
            {
                animator.Play("Attack");
                //animator.SetTrigger("AttackTrigger");
            }
        }
    }

    // アニメーションイベントから呼び出される関数
    /// <summary>
    /// 攻撃開始
    /// </summary>
    public void PerformAttack()
    {
        //slashEffect?.Clear();
        slashEffect?.Play();
        isAttacking = true;
    }
    /// <summary>
    /// 攻撃終了
    /// </summary>
    public void EndAttack()
    {
        isAttacking = false;
        _hitEnemyList.Clear();
        slashEffect?.Clear();
        slashEffect?.Stop();
    }
    /// <summary>
    /// 攻撃エフェクト再生停止
    /// </summary>
    public void StopSlashEffect()
    {
        slashEffect?.Clear();
        slashEffect?.Stop();
    }

    /// <summary>
    /// 足音SE再生
    /// </summary>
    public void PlayFootsteps()
    {
        Debug.Log("walk");
        AudioManager.Instance.PlaySE("player_Footsteps");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            if (enemy != null)
            {
                //攻撃
                if (isAttacking == true && 0 < enemy.enemyData.hp
                    && _hitEnemyList.Contains(enemy.GetInstanceID()) == false)
                {
                    enemy.Damage(attackPower);
                    _hitEnemyList.Add(enemy.GetInstanceID());
                }

                if (enemy.enemyData.hp <= 0)
                {
                    if (isPossession == false && canPossesion == true && name == "Player")
                    {
                        Possession(enemy.gameObject);
                    }
                }
                
            }
        }
    }

    public void Damage(int damage)
    {
        hp -= damage;
        _hpSlider.UpdateHPSlider();
       
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
    /// 移動キーの入力値を取得する
    /// </summary>
    private void OnMove(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
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
            playerController._hpSlider = _hpSlider;
            playerController.attackPower = currentPossession.attackPower;
            playerController.speed = 7.0f;
            playerController.player = player;
            playerController.PossessionEnemyName = currentPossession.enemyName;
            playerController.isPossession = true;
            player = null;  
            playerController.currentPossession = currentPossession;
            currentPossession = null;

            //PlayerのHPsliderを憑依体のHPに設定
            //PlayerHpSlider playerHpSlider = GameObject.Find("HP").GetComponent<PlayerHpSlider>();
            //if (playerHpSlider != null)
            //{
            //    playerHpSlider.SetPlayerHp(playerController);
            //    playerHpSlider.hpSlider.fillRect.GetComponent<Image>().color = new Color(0.8030842f, 0.4134211f, 0.9245283f, 1.0f);
            //}
            Color setColor = new Color(0.8030842f, 0.4134211f, 0.9245283f, 1.0f);
            _hpSlider.SetPlayerHp(playerController,setColor);

        }

        EnemyController enemyController = targetObj?.GetComponent<EnemyController>();
        if(enemyController != null) 
        {
            //敵のアニメーターステータスを変更
            //enemyController.animator.SetBool("IsPossession", true);

            //ライトエフェクトを削除
            enemyController.lightEffect.SetActive(false);
            enemyController.enemyData.hp = playerController.maxHp;
            Destroy(enemyController);

            // UI表示
            ActionStateManager.Instance.RecordEnemyPossession(playerController.PossessionEnemyName);
        }

        //タグをPlayerに変更
        targetObj.tag = "Player";
        targetObj.layer = gameObject.layer;
        
        //憑依体の名前に"(Player)"を追加
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(targetObj.name);
        stringBuilder.Append("(Player)");
        targetObj.name = stringBuilder.ToString();

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
    /// 憑依可能にする
    /// </summary>
    /// <param name="context">憑依ボタン</param>
    private void CanPossesion(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {
            canPossesion = true;
        }
        else if (context.canceled == true)
        {
            canPossesion = false;
        }
    }

    /// <summary>
    /// 人間に戻るアクション
    /// </summary>
    private void ReturnAction(InputAction.CallbackContext context)
    {
        if(context.performed == true)
        {
            Return();
        }
    }

    /// <summary>
    /// 人間に戻る処理
    /// </summary>
    private void Return()
    {
        if (player != null)
        {
            //"Player"(人間)を表示する
            player.SetActive(true);

            Color setColor = new Color(0.6705883f, 1.0f, 0.5803922f, 1.0f);
            _hpSlider.SetPlayerHp(player.GetComponent<PlayerController>(), setColor);

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

    /// <summary>
    /// プレイヤーの入力操作を有効、無効化する関数
    /// </summary>
    /// <param name="value">true=有効　false=無効</param>
    public void SetInputAction(bool value)
    {
        if(value == true)
        {
            inputActions.Enable();
        }
        else
        {
            inputActions.Disable();
        }
    }

    //攻撃の当たり判定を有効化する
    public void EnableHit()
    {
    }

    //攻撃の当たり判定を無効化する
    public void DisableHit()
    {
    }

}
