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
    Vector3 startPos;   // �X�|�[���n�_
    CharacterController controller;�@// �L�����N�^�[�R���g���[���[
    private Animator animator;
    public float speed = 5f;
    public float sensitivity = 30.0f;

    //�v���C���[�̍U����
    private int attackPower2 = 1;

    /// <summary>
    /// ���͗p
    /// </summary>
    private InputControls inputActions;

    private HashSet<int> _hitEnemyList;  //�U���ɐG�ꂽ�G�l�~�[���X�g

    /// <summary>
    /// �߈ˌ�ɕۑ����邽�߂̕ϐ�
    /// </summary>
    private GameObject player = null;

    /// <summary>
    /// �v���C���[�̈ړ���
    /// </summary>
    private Vector2 inputMove;

    /// <summary>
    /// �v���C���[�̉�]���x
    /// </summary>
    private float turnVelocity;

    /// <summary>
    /// �i�s�����Ɍ����̂ɂ����鎞��
    /// </summary>
    [SerializeField] private float smoothTime = 0.1f;

    private EnemyData currentPossession; // ���ݜ߈˂��Ă���G�l�~�[�̃f�[�^

    private int hp = 250;
    private int maxHp = 250;
    [SerializeField] private int defencePower = 0;      //�h���
    [SerializeField]private int attackPower = 10;       //�U����
    private int _increaseAttackValue;               //�U���͂̑����l
    private bool isAttacking = false;
    public ParticleSystem slashEffect;
    private PlayerHpSlider _hpSlider;

    public static GameOverController Instance { get; private set; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    List<Transform> _children;

    // �߈˂��Ă��邩
    public bool isPossession = false;
    private bool canPossesion = false;
    // �߈˂��Ă���G�l�~�[�̖��O���擾
    public string PossessionEnemyName;

    //�I�[�f�B�I�\�[�X
    [SerializeField]private AudioSource _audioSource;
    //se���X�g
    [SerializeField] private List<SoundData> _seData;

    //���G��Ԃ̃t���O
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

        //�q�I�u�W�F�N�g���擾����
        _children= new List<Transform>();
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
        GameObject cameraObject = GameObject.Find("Main Camera");
        _mainCamera = cameraObject?.GetComponent<CameraMovement>();
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
        //inputActinos�̃R�[���o�b�N�̉���
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Fire.performed -= AttackAnimation;
        inputActions.Player.Possession.performed -= CanPossesion;
        inputActions.Player.Possession.canceled -= CanPossesion;
        inputActions.Player.Return.performed -= ReturnAction;

        //���̓R���g���[���[�̍폜
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
            if (animator != null && !isAttacking)
            {
                animator.Play("Attack");
                //animator.SetTrigger("AttackTrigger");
            }
        }
    }

    // �A�j���[�V�����C�x���g����Ăяo�����֐�
    /// <summary>
    /// �U���J�n
    /// </summary>
    public void PerformAttack()
    {
        //slashEffect?.Clear();
        slashEffect?.Play();
        isAttacking = true;
    }
    /// <summary>
    /// �U���I��
    /// </summary>
    public void EndAttack()
    {
        isAttacking = false;
        _hitEnemyList.Clear();
        slashEffect?.Clear();
        slashEffect?.Stop();
        //if(_mainCamera.CameraState==CameraMovement.State.Shake)
        //{
        //    _mainCamera.CameraState = CameraMovement.State.Follow;
        //}
    }
    /// <summary>
    /// �U���G�t�F�N�g�Đ���~
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
    /// ����SE�Đ�
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
                //�U��
                if (isAttacking == true && 0 < enemy.enemyData.hp)
                {
                    if (_hitEnemyList.Contains(enemy.GetInstanceID()) == false)
                    {
                        enemy.Damage(attackPower);
                        _hitEnemyList.Add(enemy.GetInstanceID());
                        if (!isPossession) PlaySE("player_HitSlash");
                        else PlaySE("EnemyAttack");
                    }
                }
                //�߈�
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
            //�߈ˏ�Ԃł���ΐl�ԑ̂ɖ߂�
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
        Debug.Log("�v���C���[�����S�����I");
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

    /// <summary>
    /// �ړ��p�֐�
    /// </summary>
    private Vector3 force = new Vector3(0, 0, 0);   //�����o����
    private Vector3 forcedecay = new Vector3(0, 0, 0);   //�����o���͂̌���
    private float forcetime = 0;                      //�����o������
    private void PlayerMove()
    {
        float speedX = inputMove.x * speed;
        float speedY = inputMove.y * speed;

        //���͒l����A���ݑ��x���v�Z
        Vector3 moveVelocity = new Vector3(speedX, 0, speedY);

        //���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        Vector3 moveDelta = moveVelocity * Time.deltaTime;


        if ((inputMove != Vector2.zero || 0 < forcetime)
            && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") == false)
        {
            //�ړ�������
            controller.Move(moveDelta);

            //�����o���͂��ړ��ɉ�����
            if (0 < forcetime) controller.Move(force);

            if (player != null)
            {
                player.transform.position = transform.position;
            }

            animator.SetFloat("Speed", 1.0f);
            //�L�����N�^�[�̉�]
            MoveRotation();
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        //�����o�����ԂƗ͂����炷
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
    /// �ړ����̃L�����N�^�[�̉�]�֐�
    /// </summary>
    private void MoveRotation()
    {
        //���͒l����y������̖ڕW�p�x���v�Z
        var targetAngleY = -Mathf.Atan2(inputMove.y, inputMove.x) * Mathf.Rad2Deg + 90;
        //�ɋ}�����Ȃ��玟�̊p�x���v�Z
        var angleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnVelocity, smoothTime);
        //��]������
        transform.rotation = Quaternion.Euler(0, angleY, 0);
        if(player != null) 
        {
            player.transform.rotation= Quaternion.Euler(0, angleY, 0);
        }
    }

    //�v���C���[���m�b�N�o�b�N���� force:�����o���� Base:�m�b�N�o�b�N�̔����n�_
    public void KnockBack(float force, float time, Vector3 Base)
    {
        Vector3 PlayerPos = transform.position;

        PlayerPos.y = 0;
        Base.y = 0;

        //�v���C���[�ƑΏۊԂ̊p�x�����
        var diff = (PlayerPos - Base).normalized;
        Vector3 PushAngle = diff * (force * time);

        this.force = PushAngle;
        forcedecay = PushAngle / time;
        forcetime = time;
    }

    /// <summary>
    /// �ړ��L�[�̓��͒l���擾����
    /// </summary>
    private void OnMove(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// �߈˃A�N�V����
    /// </summary>
    /// <param name="targetObj">�߈˂���L�����N�^�[</param>
    private void Possession(GameObject targetObj)
    {
        GuideBarController.Instance.GuideSet(GuideBarController.GuideName.Pause, GuideBarController.GuideName.Return, GuideBarController.GuideName.Attack, GuideBarController.GuideName.Move);

        player = gameObject;
        //�^�O��Player�ɕύX
        targetObj.tag = "Player";
        targetObj.layer = gameObject.layer;

        SetPlayerActive(false);
        animator.SetFloat("Speed", 0);

        //�ΏۂɃv���C���[�R���g���[���[��ǉ�
        targetObj.gameObject.AddComponent<CharacterController>();
        CharacterController characterController = targetObj?.gameObject.GetComponent<CharacterController>();
        // �G�l�~�[��CapsuleCollider����R���W�����̍����E���S����̍��W�E���a�����p���Ńf�X�g���C
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
            //����Ώۂ�؂�ւ���
            playerController.enabled = true;

            //�߈˃L�����̃p�����[�^��ݒ�
            EnemyController enemy = targetObj?.GetComponent<EnemyController>();
            //currentPossession = targetObj?.GetComponent<EnemyController>().enemyData;
            currentPossession = enemy.enemyData;
            playerController.maxHp = currentPossession.Poshp;
            playerController.hp = playerController.maxHp;
            playerController._hpSlider = _hpSlider;
            playerController.attackPower = currentPossession.attackPower + _increaseAttackValue;
            playerController.defencePower = defencePower;
            playerController._audioSource= enemy.GetComponent<AudioSource>();

            playerController._seData = new List<SoundData>();
            playerController._seData = _seData;
            playerController._seData.Add(enemy.AttackSE);

            playerController.speed = 7.0f;
            playerController.player = player;
            playerController.PossessionEnemyName = currentPossession.enemyName;
            playerController.isPossession = true;
            player = null;  
            playerController.currentPossession = currentPossession;
            currentPossession = null;

            //Player��HPslider��߈ˑ̂�HP�ɐݒ�
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
            //�G�̃A�j���[�^�[�X�e�[�^�X��ύX
            //enemyController.animator.SetBool("IsPossession", true);

            //���C�g�G�t�F�N�g���폜
            enemyController.lightEffect.SetActive(false);
            enemyController.enemyData.hp = playerController.maxHp;
            Destroy(enemyController);

            // UI�\��
            ActionStateManager.Instance.RecordEnemyPossession(playerController.PossessionEnemyName);
        }

        //�߈ˑ̖̂��O��"(Player)"��ǉ�
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(targetObj.name);
        stringBuilder.Append("(Player)");
        targetObj.name = stringBuilder.ToString();

        //�߈˂����G��HP�o�[���폜
        Canvas canvas=targetObj?.GetComponentInChildren<Canvas>();
        if(canvas != null) 
        {
            canvas.enabled = false;
        }
        
        //�J�����̃^�[�Q�b�g��߈˃L�����ɐ؂�ւ���
        GameObject camera = GameObject.Find("Main Camera");
        if (camera != null)
        {
            camera.GetComponent<CameraMovement>().SetCameraTarget(targetObj);
        }
        isPossession = true;

    }

    /// <summary>
    /// �߈ˉ\�ɂ���
    /// </summary>
    /// <param name="context">�߈˃{�^��</param>
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
    /// �l�Ԃɖ߂�A�N�V����
    /// </summary>
    private void ReturnAction(InputAction.CallbackContext context)
    {
        if(context.performed == true)
        {
            Return();
        }
    }

    /// <summary>
    /// �l�Ԃɖ߂鏈��
    /// </summary>
    private void Return()
    {
        if (player != null)
        {
            GuideBarController.Instance.GuideSet(GuideBarController.GuideName.Pause, GuideBarController.GuideName.Attack, GuideBarController.GuideName.Move);

            //"Player"(�l��)��\������
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.SetPlayerActive(true);
            playerController.PlaySE("player_Return");
           
            Color setColor = new Color(0.6705883f, 1.0f, 0.5803922f, 1.0f);
            _hpSlider.SetPlayerHp(playerController, setColor);

            //�J�����̃^�[�Q�b�g��"Player"(�l��)�ɖ߂�
            GameObject camera = GameObject.Find("Main Camera");
            if (camera != null)
            {
                camera.GetComponent<CameraMovement>().SetCameraTarget(player);
            }

            //�߈ˑ̂��폜
            player = null;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �v���C���[�̕\���ݒ�֐�
    /// </summary>
    /// <param name="isActive">true=�\�� false=��\��</param>
    public void SetPlayerActive(bool isActive)
    {
        //�v���C���[�\���ݒ�
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
    /// �v���C���[�̓��͑����L���A����������֐�
    /// </summary>
    /// <param name="value">true=�L���@false=����</param>
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

    //�U���̓����蔻���L��������
    public void EnableHit()
    {
    }

    //�U���̓����蔻��𖳌�������
    public void DisableHit()
    {
    }

    /// <summary>
    /// �󔠎擾�֐�
    /// </summary>
    /// <param name="addAttack">�U���͑����l</param>
    /// <param name="addDefence">�h��͑����l</param>
    public void GetTreasure(int addAttack,int addDefence)
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

    // �U���͂𑝉������郁�\�b�h
    public void IncreaseAttackPower(int addValue)
    {
        attackPower += addValue;
        _increaseAttackValue = addValue;
    }

    //�h��͂𑝉������郁�\�b�h
    public void IncreaseDefencePower(int addValue)
    {
        defencePower += addValue;
    }

    public void PlaySE(string seFileName)
    {
        SoundData se = _seData.Find(data => data.FileName == seFileName);
        _audioSource.volume = se.Volume;
        _audioSource.PlayOneShot(se.AudioClip);
        Debug.Log($"{se.FileName}");
    }
}
