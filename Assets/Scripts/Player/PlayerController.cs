using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Vector3 startPos;   // 初期座標
    CharacterController controller;// キャラクターコントローラー
    private Animator animator;
    public float speed = 5f;
    public float sensitivity = 30.0f;

    private int attackPower2 = 1;

    /// <summary>
    /// 入力コントローラー
    /// </summary>
    private InputControls inputActions;

    private HashSet<int> _hitEnemyList;  //攻撃に触れたエネミーリスト

    /// <summary>
    /// 憑依時にプレイヤーを保存する変数
    /// </summary>
    private GameObject player = null;

    /// <summary>
    /// 移動入力値
    /// </summary>
    private Vector2 inputMove;

    /// <summary>
    /// 回転速度
    /// </summary>
    private float turnVelocity;

    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField] private float smoothTime = 0.1f;

    private EnemyData currentPossession; // 憑依キャラクター情報

    private int hp = 250;
    private int maxHp = 250;
    [SerializeField] private int defencePower = 0;      //防御力
    [SerializeField] private int attackPower = 10;       //攻撃力
    private int _increaseAttackValue;               //攻撃増加値
    private bool isAttacking = false;
    public ParticleSystem slashEffect;
    private PlayerHpSlider _hpSlider;

    public static GameOverController Instance { get; private set; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    List<Transform> _children;

    // 憑依状態化のフラグ
    public bool isPossession = false;
    private bool canPossesion = false;
    // 憑依先の名前
    public string PossessionEnemyName;

    //オーディオソース
    [SerializeField] private AudioSource _audioSource;
    //seデータ
    [SerializeField] private List<SoundData> _seData;

    //無敵状態化のフラグ
    private bool _isInfinity;
    public bool IsInfinity
    {
        get => _isInfinity;
        set => _isInfinity = value;
    }

    CameraMovement _mainCamera;

    private void Awake()
    {
        inputActions = new InputControls();

        inputActions.Enable();

        _hitEnemyList = new HashSet<int>();


        _hpSlider = GameObject.Find("HP").GetComponent<PlayerHpSlider>();

        //子オブジェクト
        _children = new List<Transform>();
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            _children.Add(transform.GetChild(i));
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        _increaseAttackValue = 0;
        startPos = transform.position;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.Play("Idle");
        slashEffect = GetComponentInChildren<ParticleSystem>();
        if (slashEffect != null)
        {
            slashEffect.Stop();
        }



        currentPossession = null;

        // �}�E�X�J�[�\�����\���ɂ���
        //Cursor.visible = false;
        //// �}�E�X�J�[�\������ʂ̒����ɌŒ肷��
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
        //inputActinosからアクションを削除
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Fire.performed -= AttackAnimation;
        inputActions.Player.Possession.performed -= CanPossesion;
        inputActions.Player.Possession.canceled -= CanPossesion;
        inputActions.Player.Return.performed -= ReturnAction;

        //inputAction削除
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
    }

    void AttackAnimation(InputAction.CallbackContext context)
    {
        if (context.performed == true && Time.timeScale != 0)
        {
            if (animator != null && !isAttacking)
            {
                animator.Play("Attack");
            }
        }
    }

    // アニメーションイベント
    /// <summary>
    /// 攻撃
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
        if (slashEffect != null)
        {
            slashEffect?.Clear();
            slashEffect?.Stop();
        }

        if (_mainCamera.CameraState == CameraMovement.State.Shake)
        {
            _mainCamera.CameraState = CameraMovement.State.Follow;
            _mainCamera.ShakeCamera(-1);
        }

    }
    /// <summary>
    /// 攻撃エフェクト停止
    /// </summary>
    public void StopSlashEffect()
    {
        slashEffect?.Clear();
        slashEffect?.Stop();
    }

    public void PlayAttackSE()
    {
        PlaySE("player_Slash");
    }

    /// <summary>
    ///足音再生
    /// </summary>
    public void PlayFootsteps()
    {
        PlaySE("player_Footsteps");
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            if (enemy != null)
            {
                //攻撃処理
                if (isAttacking == true && 0 < enemy.enemyData.hp)
                {
                    if (_hitEnemyList.Contains(enemy.GetInstanceID()) == false)
                    {
                        enemy.Damage(attackPower);
                        _hitEnemyList.Add(enemy.GetInstanceID());
                        if (!isPossession) PlaySE("player_HitSlash");
                        else PlaySE("EnemyAttack");

                        if (_mainCamera.CameraState != CameraMovement.State.Shake)
                        {
                            _mainCamera.CenterPosition = _mainCamera.transform.position;
                            _mainCamera.CameraState = CameraMovement.State.Shake;
                            _mainCamera.ShakeCamera(1);
                        }
                    }
                }
                //憑依
                if (enemy.enemyData.hp <= 0)
                {
                    if (name == "Player")
                    {
                        GuideBarController.Instance.GuideSet(GuideBarController.GuideName.Pause, GuideBarController.GuideName.Possession, GuideBarController.GuideName.Attack, GuideBarController.GuideName.Move);
                        if (isPossession == false && canPossesion == true)
                        {
                            Possession(enemy.gameObject);
                        }
                    }
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GuideBarController.Instance.RemoveGuide(GuideBarController.GuideName.Possession);
    }

    public void Damage(int damage)
    {
        if (_isInfinity == true)
        {
            return;
        }

        hp -= damage - defencePower;
        _hpSlider.UpdateHPSlider();

        if (hp <= 0)
        {
            //憑依状態であれば解除
            if (name != "Player")
            {
                Return();
            }
            else
            {
                OnDeath();
            }
        }
    }

    private void OnDeath()
    {
        animator.SetTrigger("DieTrigger");
        StartCoroutine(DestroyAfterAnimation("Die01_Stay_SwordAndShield", 0));
        GameOverController gameOverController = FindObjectOfType<GameOverController>();
        if (gameOverController != null)
        {
            gameOverController.ShowGameOverScreen();
        }
    }

    private IEnumerator DestroyAfterAnimation(string animationName, int layerIndex)
    {
        // �A�j���[�V�����̒������擾
        float animationLength = animator.GetCurrentAnimatorStateInfo(layerIndex).length;

        // �A�j���[�V��������������̂�҂�
        yield return new WaitForSeconds(animationLength);
    }


    private Vector3 force = new Vector3(0, 0, 0);
    private Vector3 forcedecay = new Vector3(0, 0, 0);
    private float forcetime = 0;
    private void PlayerMove()
    {
        float speedX = inputMove.x * speed;
        float speedY = inputMove.y * speed;


        Vector3 moveVelocity = new Vector3(speedX, 0, speedY);


        Vector3 moveDelta = moveVelocity * Time.deltaTime;


        if ((inputMove != Vector2.zero || 0 < forcetime)
            && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") == false)
        {
            //移動
            controller.Move(moveDelta);


            if (0 < forcetime) controller.Move(force);

            if (player != null)
            {
                player.transform.position = transform.position;
            }

            animator.SetFloat("Speed", 1.0f);
            //回転
            MoveRotation();
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }


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
    /// プレイヤーの回転
    /// </summary>
    private void MoveRotation()
    {
        //入力値から回転値を取得
        var targetAngleY = -Mathf.Atan2(inputMove.y, inputMove.x) * Mathf.Rad2Deg + 90;

        var angleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0, angleY, 0);
        if (player != null)
        {
            player.transform.rotation = Quaternion.Euler(0, angleY, 0);
        }
    }


    public void KnockBack(float force, float time, Vector3 Base)
    {
        Vector3 PlayerPos = transform.position;

        PlayerPos.y = 0;
        Base.y = 0;


        var diff = (PlayerPos - Base).normalized;
        Vector3 PushAngle = diff * (force * time);

        this.force = PushAngle;
        forcedecay = PushAngle / time;
        forcetime = time;
    }

    /// <summary>
    /// 移動入力を受け取る
    /// </summary>
    private void OnMove(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 憑依処理
    /// </summary>
    /// <param name="targetObj">憑依対象[</param>
    private void Possession(GameObject targetObj)
    {
        GuideBarController.Instance.GuideSet(GuideBarController.GuideName.Pause, GuideBarController.GuideName.Return, GuideBarController.GuideName.Attack, GuideBarController.GuideName.Move);

        player = gameObject;
        //憑依先のタグ、レイヤーをプレイヤーにする
        targetObj.tag = "Player";
        targetObj.layer = gameObject.layer;

        SetPlayerActive(false);
        animator.SetFloat("Speed", 0);

        //キャラクターコントローラーの設定
        targetObj.gameObject.AddComponent<CharacterController>();
        CharacterController characterController = targetObj?.gameObject.GetComponent<CharacterController>();

        CapsuleCollider capsuleCollider = targetObj?.gameObject.GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            characterController.height = capsuleCollider.height;
            characterController.center = capsuleCollider.center;
            characterController.radius = capsuleCollider.radius;
            capsuleCollider.enabled = true;
        }
        targetObj.gameObject.AddComponent<PlayerController>();

        PlayerController playerController = targetObj?.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.enabled = true;

            //憑依キャラの設定
            EnemyController enemy = targetObj?.GetComponent<EnemyController>();

            currentPossession = enemy.enemyData;
            playerController.maxHp = currentPossession.Poshp;
            playerController.hp = playerController.maxHp;
            playerController._hpSlider = _hpSlider;
            playerController.attackPower = currentPossession.attackPower + _increaseAttackValue;
            playerController.defencePower = defencePower;
            playerController._audioSource = enemy.GetComponent<AudioSource>();

            playerController._seData = new List<SoundData>();
            playerController._seData = _seData;
            playerController._seData.Add(enemy.AttackSE);

            playerController.speed = 7.0f;
            playerController.player = player;
            playerController.PossessionEnemyName = currentPossession.enemyName;
            playerController.isPossession = true;
            player = null;
            playerController.currentPossession = currentPossession;
            playerController._mainCamera = _mainCamera;
            currentPossession = null;

            Color setColor = new Color(0.8030842f, 0.4134211f, 0.9245283f, 1.0f);
            _hpSlider.SetPlayerHp(playerController, setColor);

        }

        EnemyController enemyController = targetObj?.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            //エネミーの死亡時エフェクト削除
            enemyController.lightEffect.SetActive(false);
            enemyController.enemyData.hp = playerController.maxHp;
            Destroy(enemyController);

            // 憑依UI設定
            ActionStateManager.Instance.RecordEnemyPossession(playerController.PossessionEnemyName);
        }

        //憑依先に名前追加
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(targetObj.name);
        stringBuilder.Append("(Player)");
        targetObj.name = stringBuilder.ToString();

        //憑依先のキャンバス削除
        Canvas canvas = targetObj?.GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = false;
        }

        //カメラのターゲットを憑依先に変更
        _mainCamera.SetCameraTarget(targetObj);
        isPossession = true;

    }

    /// <summary>
    /// 憑依入力処理
    /// </summary>
    /// <param name="context">入力値</param>
    private void CanPossesion(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {
            PlaySE("player_Possesion");
            canPossesion = true;
        }
        else if (context.canceled == true)
        {
            canPossesion = false;
        }
    }

    /// <summary>
    /// 憑依解除入力処理
    /// </summary>
    private void ReturnAction(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {
            Return();
        }
    }

    /// <summary>
    /// 憑依解除
    /// </summary>
    private void Return()
    {
        if (player != null)
        {
            GuideBarController.Instance.GuideSet(GuideBarController.GuideName.Pause, GuideBarController.GuideName.Attack, GuideBarController.GuideName.Move);

            //プレイヤーを有効化
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.SetPlayerActive(true);
            playerController.PlaySE("player_Return");

            Color setColor = new Color(0.6705883f, 1.0f, 0.5803922f, 1.0f);
            _hpSlider.SetPlayerHp(playerController, setColor);

            //カメラのターゲットをプレイヤーに戻す
            _mainCamera.SetCameraTarget(player);

            player = null;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// プレイヤーの表示設定
    /// </summary>
    /// <param name="isActive">true=表示 false=非表示</param>
    public void SetPlayerActive(bool isActive)
    {
        //子オブジェクトを取得
        foreach (var child in _children)
        {
            child.gameObject.SetActive(isActive);
        }
        GetComponent<CapsuleCollider>().enabled = isActive;
        enabled = isActive;
        if (isActive == true)
        {
            tag = "Player";
            gameObject.layer = 3;
        }
        else
        {
            tag = "Untagged";
            gameObject.layer = 0;
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
    /// プレイヤーの入力操作の有効切り替え
    /// </summary>
    /// <param name="value">true=�L���@false=����</param>
    public void SetInputAction(bool value)
    {
        if (value == true)
        {
            inputActions.Enable();
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.Find("Main Camera").GetComponent<CameraMovement>();
            }
        }
        else
        {
            inputActions.Disable();
        }
    }

    public void EnableHit()
    {
    }

    public void DisableHit()
    {
    }

    /// <summary>
    /// 宝箱を取ったとき
    /// </summary>
    /// <param name="addAttack">攻撃力増加量</param>
    /// <param name="addDefence">防御力増加量</param>
    public void GetTreasure(int addAttack, int addDefence)
    {
        IncreaseAttackPower(addAttack);
        IncreaseDefencePower(addDefence);
        hp = maxHp;
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.hp = playerController.maxHp;
        }
        _hpSlider.UpdateHPSlider();
    }

    // 攻撃力増加処理
    public void IncreaseAttackPower(int addValue)
    {
        attackPower += addValue;
        _increaseAttackValue = addValue;
    }

    //防御力増加処理
    public void IncreaseDefencePower(int addValue)
    {
        defencePower += addValue;
    }

    public void PlaySE(string seFileName)
    {
        SoundData se = _seData.Find(data => data.FileName == seFileName);
        _audioSource.volume = se.Volume;
        _audioSource.PlayOneShot(se.AudioClip);
    }
}
