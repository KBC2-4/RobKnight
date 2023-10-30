using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 startPos;   // スポーン地点
    CharacterController controller;　// キャラクターコントローラー
    private Animator animator;
    public float speed = 5f;
    public float sensitivity = 30.0f;
    float rotX, rotY;
    public EnemyData currentPossession; // 現在憑依しているエネミーのデータ
    private GameObject currentModel;

    public int hp = 100;
    public int maxHp = 100;
    public int mp = 100;
    public int attackDamage = 10;
    private bool isAttacking = false;
    public ParticleSystem particleSystem;
    public float possessionRange = 3.0f; // 憑依可能な距離
    public GameObject possessionUI; // 憑依を促すUI

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

        // マウスカーソルを非表示にする
        //Cursor.visible = false;
        //// マウスカーソルを画面の中央に固定する
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

        // コントローラーの右ステックの入力を取得
        float rightStickHorizontal = Input.GetAxis("RightHorizontal");
        float rightStickVertical = Input.GetAxis("RightVertical");

        // マウスの座標を取得
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

        // ブレンドツリー
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
        //Debug.Log("Enemyかな？");
        if (isAttacking && other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemyだ！”攻撃開始");
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
}
