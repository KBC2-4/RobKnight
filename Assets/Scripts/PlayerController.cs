using System;
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

    /// <summary>
    /// 前フレームの座標
    /// </summary>
    private Vector3 prevPosition;

    /// <summary>
    /// 現在の回転速度
    /// </summary>
    private float currentAngleVelocity;

    /// <summary>
    /// 回転速度の最大値
    /// </summary>
    [SerializeField] private float maxAngularSpeed = Mathf.Infinity;
    /// <summary>
    /// 進行方向に向くのにかかる時間
    /// </summary>
    [SerializeField] private float smoothTime = 0.1f;

    float rotX, rotY;
    public EnemyData currentPossession; // 現在憑依しているエネミーのデータ
    private GameObject currentModel;

    public int hp = 100;
    public int maxHp = 100;
    public int mp = 100;
    public int attackDamage = 10;
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
           // Possess(currentPossession);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        //キャラクターの回転
        MoveRotation();

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

    //private void FixedUpdate()
    //{
    //    animator.SetFloat("Speed", movement.magnitude);
    //}

    void AttackAnimation()
    {
        //animator.SetBool("AttackBool",true);
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
        //Debug.Log("Enemyかな？");
        if (other.gameObject.CompareTag("Enemy"))
        {
            //攻撃処理
            if (isAttacking)
            {
                Debug.Log("Enemyだ！”攻撃開始");
                EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    Debug.Log("attack");
                    enemyController.Damage(attackDamage);
                    isAttacking = false;
                }
            } //憑依処理
            else if(Input.GetButtonDown("Fire2"))
            {
                Debug.Log("hyoui");
                Possession(other.gameObject);             
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

    /// <summary>
    /// 移動時のキャラクターの回転関数
    /// </summary>
    private void MoveRotation()
    {


        //現在座標の取得
        Vector3 position = transform.position;

        //移動量の計算
        Vector3 deltaMove = position - prevPosition;

        //前フレームの座標の更新
        prevPosition = position;

        //静止している場合回転させない
        if (deltaMove == Vector3.zero)
        {
            return;
        }

        //進行方向に向くような回転を取得
        Quaternion rotation = Quaternion.LookRotation(deltaMove, Vector3.up);

        //現在の向きと進行方向の角度差を計算
        float diffAngle = Vector3.Angle(transform.forward, deltaMove);

        //現在フレームで回転する角度の計算
        float rotaAngle = Mathf.SmoothDampAngle(0, diffAngle, ref currentAngleVelocity, smoothTime, maxAngularSpeed);

        //現在フレームにおける回転を計算
        Quaternion nextRota = Quaternion.RotateTowards(transform.rotation, rotation, rotaAngle);

        //回転させる
        transform.rotation = nextRota;
    }

    /// <summary>
    /// 憑依アクション
    /// </summary>
    /// <param name="targetObj">憑依するキャラクター</param>
    private void Possession(GameObject targetObj)
    {
        //現在の体カラ操作機能を失効させる
        GetComponent<PlayerController>().enabled = false;

        //対象にプレイヤーコントローラーを追加
        targetObj.gameObject.AddComponent<CharacterController>();
        targetObj.gameObject.AddComponent<PlayerController>();

        //操作対象を切り替える
        targetObj.GetComponent<PlayerController>().enabled = true;

        //憑依キャラのパラメータを設定
        targetObj.GetComponent<PlayerController>().maxHp = targetObj.GetComponent<EnemyData>().maxHp;
        targetObj.GetComponent<PlayerController>().hp = targetObj.GetComponent<PlayerController>().maxHp;
        targetObj.GetComponent<PlayerController>().attackDamage = targetObj.GetComponent<EnemyData>().attackPower;

        //カメラのターゲットを憑依キャラに切り替える
        GameObject camera = GameObject.Find("MainCamera");
        if (camera != null)
        {
            camera.GetComponent<CameraMovement>().SetCameraTarget(targetObj);
        }
    }
}
