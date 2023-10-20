using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    public float speed = 5f;
    public float sensitivity = 30.0f;
    float rotX, rotY;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject!");
        }

        // マウスカーソルを非表示にする
        Cursor.visible = false;
        // マウスカーソルを画面の中央に固定する
        Cursor.lockState = CursorLockMode.Locked;


        if (Application.isEditor)
        {
            sensitivity = sensitivity * 1.5f;
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
        Vector3 movement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
        transform.Translate(movement);

        if (Input.GetMouseButton(0))
        {
            AttackAnimation();
        }
    }

    void AttackAnimation()
    {
        animator.SetBool("AttackBool",true);
    }
}
