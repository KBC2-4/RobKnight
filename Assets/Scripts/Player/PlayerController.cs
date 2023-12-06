using System.Collections;
using System.Collections.Generic;
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
    public int attackPower = 10;
    private bool isAttacking = false;
    public ParticleSystem slashEffect;
    private PlayerHpSlider _hpSlider;

    public static GameOverController Instance { get; private set; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    // �߈˂��Ă��邩
    public bool isPossession = false;
    private bool canPossesion = false;
    // �߈˂��Ă���G�l�~�[�̖��O���擾
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
            // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
            //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

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
    }
    /// <summary>
    /// �U���G�t�F�N�g�Đ���~
    /// </summary>
    public void StopSlashEffect()
    {
        slashEffect?.Clear();
        slashEffect?.Stop();
    }

    /// <summary>
    /// ����SE�Đ�
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
                //�U��
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
        //"Player"(�l��)�ł���Δ�\���ɂ���
        if (name == "Player")
        {
            player = gameObject;
            gameObject.SetActive(false);
        }//�߈ˑ̂ł���Δj������
        else
        {
           
            Destroy(gameObject);
        }
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
            //Destroy(targetObj.GetComponent<CapsuleCollider>());
        }
        targetObj.gameObject.AddComponent<PlayerController>();

        PlayerController playerController = targetObj?.GetComponent<PlayerController>();

        if (playerController != null)
        {
            //����Ώۂ�؂�ւ���
            playerController.enabled = true;

            //�߈˃L�����̃p�����[�^��ݒ�
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

        //�^�O��Player�ɕύX
        targetObj.tag = "Player";
        targetObj.layer = gameObject.layer;
        
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
        GameObject camera = GameObject.Find("MainCamera");
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
            //"Player"(�l��)��\������
            player.SetActive(true);

            Color setColor = new Color(0.6705883f, 1.0f, 0.5803922f, 1.0f);
            _hpSlider.SetPlayerHp(player.GetComponent<PlayerController>(), setColor);

            //�J�����̃^�[�Q�b�g��"Player"(�l��)�ɖ߂�
            GameObject camera = GameObject.Find("MainCamera");
            if (camera != null)
            {
                camera.GetComponent<CameraMovement>().SetCameraTarget(player);
            }

            //�߈ˑ̂��폜
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

}
