using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトがプレイヤーの場合
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーコントローラーを取得
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            Debug.Log("プレイヤーが破壊しようとしています");

            // プレイヤーが憑依している状態か確認します。
            if (playerController != null && playerController.isPossession)
            {
                if (playerController.PossessionEnemyName == "Gobrin")
                {
                    // 憑依しているエネミーがゴブリンの場合オブジェクトを破壊
                    Destroy(gameObject);
                }
            }
        }
    }
}
