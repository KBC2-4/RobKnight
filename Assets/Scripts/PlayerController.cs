using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 startPos;   // �X�|�[���n�_
    CharacterController controller;�@// �L�����N�^�[�R���g���[���[
    private Animator animator;
    public float speed = 5f;
    public float sensitivity = 30.0f;
    float rotX, rotY;
    public EnemyData currentPossession; // ���ݜ߈˂��Ă���G�l�~�[�̃f�[�^
    private GameObject currentModel;

    public int hp = 100;
    public int maxHp = 100;
    public int mp = 100;
    public int attackDamage = 10;
    private bool isAttacking = false;
    public ParticleSystem particleSystem;
    public float possessionRange = 3.0f; // �߈ˉ\�ȋ���
    public GameObject possessionUI; // �߈˂𑣂�UI

    public static GameOverController GameOverInstance { get; private set; }

    public static PlayerController PlayerInstance { get; private set; }

    void Awake()
    {
        if (PlayerInstance == null)
        {
            PlayerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Stop();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject!");
        }

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
            Possess(currentPossession);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPossessionOpportunity();
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

        // �u�����h�c���[
        //animator.SetFloat("Horizontal", horizontal);
        //animator.SetFloat("Vertical", vertical);
        //animator.SetFloat("Speed", movement.magnitude);


        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            AttackAnimation();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            isAttacking = false;
        }

        //if (Input.GetButtonUp("Fire2") && currentPossession.abilities.Length > 0)
        //{
        //    currentPossession.abilities[0].Use(transform);
        //}

        if (controller.isGrounded)
        {

        }
    }

    private void CheckForPossessionOpportunity()
    {
        foreach (var enemy in FindObjectsOfType<EnemyController>())
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (enemy.isDeath && distance < possessionRange)
            {
                possessionUI.SetActive(true);
                return;
            }
        }
        possessionUI.SetActive(false);
    }

    //private void FixedUpdate()
    //{
    //    animator.SetFloat("Speed", movement.magnitude);
    //}

    void AttackAnimation()
    {
        //animator.SetBool("AttackBool",true);
        animator.SetTrigger("Attack");
        if (particleSystem.isStopped)
        {
            particleSystem.Play();
        }
       
    }

    public void Possess(EnemyData newEnemy)
    {
        currentPossession = newEnemy;
        hp = newEnemy.maxHp;
        maxHp = hp;

        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        currentModel = Instantiate(newEnemy.modelPrefab, transform.position, transform.rotation);
        currentModel.transform.parent = this.transform;
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Enemy���ȁH");
        if (isAttacking && other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy���I�h�U���J�n");
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.Damage(attackDamage);
                isAttacking = false;
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
}
