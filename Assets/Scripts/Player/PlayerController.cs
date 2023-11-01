using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    /// �U���p���͕ϐ�
    /// </summary>
    private InputAction fireAction;

    /// <summary>
    /// �߈˗p���͕ϐ�
    /// </summary>
    private InputAction possedAction;

    /// <summary>
    /// �O�t���[���̍��W
    /// </summary>
    private Vector3 prevPosition;

    /// <summary>
    /// ���݂̉�]���x
    /// </summary>
    private float currentAngleVelocity;

    /// <summary>
    /// ��]���x�̍ő�l
    /// </summary>
    [SerializeField] private float maxAngularSpeed = Mathf.Infinity;
    /// <summary>
    /// �i�s�����Ɍ����̂ɂ����鎞��
    /// </summary>
    [SerializeField] private float smoothTime = 0.1f;

    float rotX, rotY;
    public EnemyData currentPossession; // ���ݜ߈˂��Ă���G�l�~�[�̃f�[�^
    private GameObject currentModel;


    /// <summary>
    /// �߈ˉ\���̃t���O
    /// </summary>
    private bool isPossed;

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
        prevPosition = startPos;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        //particleSystem.Stop();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject!");
        }

        //inputActions����[Fire�A�N�V����]���擾
        fireAction = inputActions.FindActionMap("Player").FindAction("Fire");
        //[Fire]�A�N�V��������Ă΂��֐���ݒ�
        fireAction.performed += _ => AttackAnimation();
        fireAction.Enable();

        //inputActions����[Fire�A�N�V����]���擾
        possedAction = inputActions.FindActionMap("Player").FindAction("Possession");
        //[Fire]�A�N�V��������Ă΂��֐���ݒ�
        //possedAction.performed += _ => SetIsPossed();
        possedAction.Enable();

        // �}�E�X�J�[�\�����\���ɂ���
        //Cursor.visible = false;
        //// �}�E�X�J�[�\������ʂ̒����ɌŒ肷��
        //Cursor.lockState = CursorLockMode.Locked;


        if (Application.isEditor)
        {
            sensitivity = sensitivity * 1.5f;
        }

        if (currentPossession != null)
        {
           // Possess(currentPossession);
        }
    }

    private void OnEnable()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // �R���g���[���[�̉E�X�e�b�N�̓��͂��擾
        float rightStickHorizontal = Input.GetAxis("RightHorizontal");
        float rightStickVertical = Input.GetAxis("RightVertical");

        // �}�E�X�̍��W���擾
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotX = (mouseX/* + rightStickHorizontal*/) * sensitivity;
        rotY = (mouseY/* + rightStickVertical*/) * sensitivity;

        //CameraRotation(cam, rotX, rotY);
        //Vector3 movement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
        //transform.Translate(movement);
        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        controller.Move(movement);

        //�L�����N�^�[�̉�]
        MoveRotation();

        // �u�����h�c���[
        //animator.SetFloat("Horizontal", horizontal);
        //animator.SetFloat("Vertical", vertical);
        //animator.SetFloat("Speed", movement.magnitude);


        //if (Input.GetButtonDown("Fire1"))
        //{
        //    isAttacking = true;
        //    AttackAnimation();
        //}

        if (Input.GetButtonUp("Fire1"))
        {
            isAttacking = false;
        }

        //if (Input.GetButtonUp("Fire2") && currentPossession.abilities.Length > 0)
        //{
        //    currentPossession.abilities[0].Use(transform);
        //}


        if(fireAction.IsPressed()==false)
        {
            isAttacking = false;
        }
    }

    //private void FixedUpdate()
    //{
    //    animator.SetFloat("Speed", movement.magnitude);
    //}

    void AttackAnimation()
    {
        isAttacking = true;
        Debug.Log("Attack");
        animator.SetTrigger("Attack");
        if (particleSystem != null && particleSystem.isStopped)
        {
            particleSystem.Play();
        }
       
    }


    //public void Possess(EnemyData newEnemy)
    //{
    //    currentPossession = newEnemy;

    //    if (currentModel != null)
    //    {
    //        Destroy(currentModel);
    //    }

    //    currentModel = Instantiate(newEnemy.modelPrefab, transform.position, transform.rotation);
    //    currentModel.transform.parent = this.transform;
    //}

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Enemy���ȁH");
        if (other.gameObject.CompareTag("Enemy"))
        {
            //�U������
            if (isAttacking)
            {
                Debug.Log("Enemy���I�h�U���J�n");
                EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    Debug.Log("attack");
                    enemyController.Damage(attackPower);
                    isAttacking = false;
                }
            } //�߈ˏ���
            else if (possedAction.IsPressed() && other.GetComponent<EnemyController>().enemyData.hp <= 0)
            {
                Debug.Log("hyoui");
                Possession(other.gameObject);
                isPossed = false;
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
    /// �ړ����̃L�����N�^�[�̉�]�֐�
    /// </summary>
    private void MoveRotation()
    {


        //���ݍ��W�̎擾
        Vector3 position = transform.position;

        //�ړ��ʂ̌v�Z
        Vector3 deltaMove = position - prevPosition;

        //�O�t���[���̍��W�̍X�V
        prevPosition = position;

        //�Î~���Ă���ꍇ��]�����Ȃ�
        if (deltaMove == Vector3.zero)
        {
            return;
        }

        //�i�s�����Ɍ����悤�ȉ�]���擾
        Quaternion rotation = Quaternion.LookRotation(deltaMove, Vector3.up);

        //���݂̌����Ɛi�s�����̊p�x�����v�Z
        float diffAngle = Vector3.Angle(transform.forward, deltaMove);

        //���݃t���[���ŉ�]����p�x�̌v�Z
        float rotaAngle = Mathf.SmoothDampAngle(0, diffAngle, ref currentAngleVelocity, smoothTime, maxAngularSpeed);

        //���݃t���[���ɂ������]���v�Z
        Quaternion nextRota = Quaternion.RotateTowards(transform.rotation, rotation, rotaAngle);

        //��]������
        transform.rotation = nextRota;
    }

    /// <summary>
    /// �߈ˉ\�ɂ���֐�
    /// </summary>
    private void SetIsPossed()
    {
        Debug.Log("hyoui");
        isPossed = false;
    }

    /// <summary>
    /// �߈˃A�N�V����
    /// </summary>
    /// <param name="targetObj">�߈˂���L�����N�^�[</param>
    private void Possession(GameObject targetObj)
    {
        //���݂̑̃J������@�\������������
        GetComponent<PlayerController>().enabled = false;

        //�ΏۂɃv���C���[�R���g���[���[��ǉ�
        targetObj.gameObject.AddComponent<CharacterController>();
        targetObj.gameObject.AddComponent<PlayerController>();

        //����Ώۂ�؂�ւ���
        targetObj.GetComponent<PlayerController>().enabled = true;

        //�߈˃L�����̃p�����[�^��ݒ�
        currentPossession = targetObj.GetComponent<EnemyController>().enemyData;
        targetObj.GetComponent<PlayerController>().maxHp = currentPossession.maxHp;
        targetObj.GetComponent<PlayerController>().hp = targetObj.GetComponent<PlayerController>().maxHp;
        targetObj.GetComponent<PlayerController>().attackPower = currentPossession.attackPower;
        targetObj.GetComponent<PlayerController>().inputActions = inputActions;

        //�^�O��Player�ɕύX
        targetObj.tag = "Player";

        Debug.Log(targetObj.tag);

        //�J�����̃^�[�Q�b�g��߈˃L�����ɐ؂�ւ���
        GameObject camera = GameObject.Find("MainCamera");
        if (camera != null)
        {
            camera.GetComponent<CameraMovement>().SetCameraTarget(targetObj);
        }
    }
}
