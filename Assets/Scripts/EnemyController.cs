using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    }

    public void OnMouseDown()
    {
        // ゲーム中にエネミーをクリックすると、プレイヤーが憑依する
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.Possess(enemyData);
        }
    }
}
