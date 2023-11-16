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
                        // 憑依しているエネミーがゴブリンの場合オブジェクトを破壊
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
