using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private InputAction possedAction;

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
    public EnemyData currentPossession; // ���ݜ߈˂��Ă���G�l�~�[�̃f�[�^
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

        //inputActions����[�ړ��A�N�V����]���擾
        moveAction = inputActions.FindActionMap("Player").FindAction("Move");
        moveAction.Enable();

        //inputActions����[�U���A�N�V����]���擾
        fireAction = inputActions.FindActionMap("Player").FindAction("Fire");
        //[�U��]�A�N�V��������Ă΂��֐���ݒ�
        fireAction.performed += _ => AttackAnimation();
        fireAction.Enable();

        //inputActions����[�߈˃A�N�V����]���擾
        possedAction = inputActions.FindActionMap("Player").FindAction("Possession");
        possedAction.Enable();
 

        // �}�E�X�J�[�\�����\���ɂ���
        //Cursor.visible = false;
        //// �}�E�X�J�[�\������ʂ̒����ɌŒ肷��
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
        //Debug.Log("Enemy���ȁH");
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            if (enemy != null)
            {
                //�U������
                if (isAttacking)
                {
                    Debug.Log("Enemy���I�h�U���J�n");
                    enemy.Damage(attackPower);
                    isAttacking = false;
                }//�߈ˏ���
                else if (possedAction.IsPressed() && enemy.enemyData.hp <= 0
                         && currentPossession == null)
                {
                    Debug.Log("prossession(�߈�)");
                    Possession(other.gameObject);
                }
            }
        }
    }

    public void Damage(int damage)
    {
        //Debug.Log("�G�l�~�[����U������Ă��܂�");
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
    private void PlayerMove()
    {
        inputMove = moveAction.ReadValue<Vector2>();

        float speedX = inputMove.x * speed;
        float speedY = inputMove.y * speed;

        //���͒l����A���ݑ��x���v�Z
        Vector3 moveVelocity = new Vector3(speedX, 0, speedY);

        //���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        Vector3 moveDelta = moveVelocity * Time.deltaTime;

        //�ړ�������
        controller.Move(moveDelta);

        //�L�����N�^�[�̉�]
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
    }

    /// <summary>
    /// �߈˃A�N�V����
    /// </summary>
    /// <param name="targetObj">�߈˂���L�����N�^�[</param>
    private void Possession(GameObject targetObj)
    {
        //���݂̑̂��瑀��@�\������������
        enabled = false;
        animator.enabled = false;

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
            Destroy(targetObj.GetComponent<CapsuleCollider>());
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
        }

        targetObj.GetComponent<EnemyController>().enabled = false;
            
        //�^�O��Player�ɕύX
        targetObj.tag = "Player";

        //�J�����̃^�[�Q�b�g��߈˃L�����ɐ؂�ւ���
        GameObject camera = GameObject.Find("MainCamera");
        if (camera != null)
        {
            camera.GetComponent<CameraMovement>().SetCameraTarget(targetObj);
        }
    }
}
