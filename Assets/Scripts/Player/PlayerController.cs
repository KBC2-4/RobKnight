using System.Collections;
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
    public InputActionAsset inputActions;

    /// <summary>
    /// �ړ��p���͕ϐ�
    /// </summary>
    private InputAction moveAction;

    /// <summary>
    /// �U���p���͕ϐ�
    /// </summary>
    private InputAction fireAction;

    /// <summary>
    /// �߈˗p���͕ϐ�
    /// </summary>
    private InputAction possessionAction;

    /// <summary>
    /// �l�Ԃɖ߂�p���͕ϐ�
    /// </summary>
    private InputAction returnAction;

    /// <summary>
    /// �l�Ԃɖ߂����Ƃ��̍��W
    /// </summary>
    private Vector3 returnPosition = new Vector3(300f, 1f, 400f);

    /// <summary>
    /// �߈˃A�N�V�������̓^�C�}�[
    /// </summary>
    private float inputTimerPossession;

    /// <summary>
    /// �߈˃A�N�V�����ɕK�v�ȓ��͎���
    /// </summary>
    public const float inputTimePossession = 0.3f;

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

    float rotX, rotY;
    private EnemyData currentPossession; // ���ݜ߈˂��Ă���G�l�~�[�̃f�[�^
    private GameObject currentModel;

    private int hp = 250;
    private int maxHp = 250;
    public int mp = 100;
    public int attackPower = 10;
    private bool isAttacking = false;
    public ParticleSystem particleSystem;

    public static GameOverController Instance { get; private set; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    // �߈˂��Ă��邩
    public bool isPossession = false;
    // �߈˂��Ă���G�l�~�[�̖��O���擾
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

        //inputActions����[�ړ��A�N�V����]���擾
        moveAction = inputActions.FindActionMap("Player").FindAction("Move");
        moveAction.Enable();

        //inputActions����[�U���A�N�V����]���擾
        fireAction = inputActions.FindActionMap("Player").FindAction("Fire");
        //[�U��]�A�N�V��������Ă΂��֐���ݒ�
        fireAction.performed += _ => AttackAnimation();
        fireAction.Enable();

        //inputActions����[�߈˃A�N�V����]���擾
        possessionAction = inputActions.FindActionMap("Player").FindAction("Possession");
        possessionAction.Enable();

        //inputActions����[�l�Ԃɖ߂�A�N�V����]���擾
        returnAction = inputActions.FindActionMap("Player").FindAction("Return");
        returnAction.performed += _ => Return();
        returnAction.Enable();

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
        // ���ݍĐ����̃A�j���[�V�����̏�Ԃ��擾
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

    // �A�j���[�V�����C�x���g����Ăяo�����֐�
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
                //�U��
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
        inputMove = moveAction.ReadValue<Vector2>();

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
    /// �߈˃A�N�V����
    /// </summary>
    /// <param name="targetObj">�߈˂���L�����N�^�[</param>
    private void Possession(GameObject targetObj)
    {
        //"Player"(�l��)�ł���Δ�\���ɂ���
        if (name == "Player")
        {
            inputTimerPossession = 0f;
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
            playerController.attackPower = currentPossession.attackPower;
            playerController.inputActions = inputActions;
            playerController.player = player;
            playerController.PossessionEnemyName = currentPossession.enemyName;
            player = null;
            playerController.currentPossession = currentPossession;
            currentPossession = null;

            //Player��HPslider��߈ˑ̂�HP�ɐݒ�
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
            //�G�̃A�j���[�^�[�X�e�[�^�X��ύX
            enemyController.animator.SetBool("IsPossession", true);

            //���C�g�G�t�F�N�g���폜
            enemyController.lightEffect.SetActive(false);
            enemyController.enemyData.hp = playerController.maxHp;
            Destroy(enemyController);
        }

        //�^�O��Player�ɕύX
        targetObj.tag = "Player";
        targetObj.layer = gameObject.layer;

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
    /// �l�Ԃɖ߂�A�N�V����
    /// </summary>
    private void Return()
    {
        if (player != null)
        {
            //"Player"(�l��)��\������
            player.SetActive(true);

            //Player��HPslider�����ɖ߂�
            PlayerHpSlider playerHpSlider = GameObject.Find("HP").GetComponent<PlayerHpSlider>();
            if (playerHpSlider != null)
            {
                playerHpSlider.SetPlayerHp(player.GetComponent<PlayerController>());
                playerHpSlider.hpSlider.fillRect.GetComponent<Image>().color = new Color(0.6705883f, 1.0f, 0.5803922f, 1.0f);
            }

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
}   
