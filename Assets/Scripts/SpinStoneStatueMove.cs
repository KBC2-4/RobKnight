using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinStoneStatueMove : MonoBehaviour
{
    public button Button;

    public bool push_flg;
    public bool isAttacked; //攻撃を食らったのか
    int count; //押されている間＋されていく

    public float power = 1f;  //押されたときにかかる力
    [SerializeField] private AudioSource _audioSorce; // 再生するSE

    // Start is called before the first frame update
    void Start()
    {
        push_flg = false;
        isAttacked = false;
        _audioSorce = GetComponent<AudioSource>();
    }

    private float forcetime = 0;                      //押し出す時間
    // Update is called once per frame
    void Update()
    {
        if (0 < forcetime && !push_flg)
        {
            transform.position += -transform.up * Time.deltaTime * forcetime * power;
        }
        else if (!push_flg)
        {
            transform.Rotate(0f, 0f, 90 * Time.deltaTime); //回転
        }
        else 
        {
            _audioSorce.Stop();
        }

        //押し出す時間を減らす
        forcetime -= Time.deltaTime;
        if (forcetime < 0)
        {
            forcetime = 0;
            _audioSorce.Stop();
        }

        transform.position = (new Vector3(Mathf.Clamp(transform.position.x, 347, 372), transform.position.y, Mathf.Clamp(transform.position.z, 707, 729)));
    }

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
                    if (playerController.IsAttacking == true)
                    {
                        //押される時間を設定する
                        forcetime = 1;
                        _audioSorce.Play();
                    }
                }
            }
        }
    }
}