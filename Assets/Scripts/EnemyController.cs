using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDeath() // エネミーが倒されたときの処理
    {
        // ここでプレイヤーに憑依オプションを提示するロジックを追加
        // 例: UIの表示や、近くのプレイヤーオブジェクトを検索してPossessメソッドを呼び出す
    }

    private void OnMouseDown()
    {
        // ゲーム中にエネミーをクリックすると、プレイヤーが憑依する
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.Possess(enemyData);
        }
    }
}
