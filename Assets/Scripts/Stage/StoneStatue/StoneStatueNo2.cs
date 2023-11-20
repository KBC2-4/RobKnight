using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneStatueNo2 : MonoBehaviour
{
    public bool isAttacked = false; //攻撃を食らったのか
    public bool during_rotation = false; //回転中なのか

    private float _old_rotaey = 0;

    private float _fixation = 230; //指定した角度になったら回転を停止

    public bool end = false; //オブジェクト終了

    public GameObject obj;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

        if (_fixation > transform.localEulerAngles.y)
        {


            if (during_rotation)
            {
                if (_old_rotaey + 15 >= transform.localEulerAngles.y)
                {
                    transform.Rotate(0f, 0f, 10 * Time.deltaTime); //回転
                }
                else
                {
                    during_rotation = false;
                }
            }
        }
        else
        {
            if (end == false)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);

                }
                end = true;
            }

        }


    }

    // Update is called once per frame

    void OnTriggerStay(Collider other)
    {
        // 衝突したオブジェクトがプレイヤーの場合
        if (other.gameObject.CompareTag("Player"))
        {
            // プレイヤーコントローラーを取得
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            // プレイヤーが憑依している状態か確認します。
            if (playerController != null)
            {
                if (playerController.PossessionEnemyName == "Gobrin")
                {
                    if (playerController.IsAttacking == true && during_rotation == false)
                    {
                        _old_rotaey = transform.localEulerAngles.y;
                        during_rotation = true;
                        isAttacked = false;
                    }
                }
            }
        }
    }
}