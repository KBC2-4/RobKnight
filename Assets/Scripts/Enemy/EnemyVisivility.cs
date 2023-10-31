using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisivility : MonoBehaviour
{

    private GameObject rootobj = null;

    // Start is called before the first frame update
    void Start()
    {
        //親オブジェクトを取得
        rootobj = transform.root.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }
    // 当たり判定がプレイヤーに触れたときの処理
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && rootobj.CompareTag("Enemy"))
        {
            EnemyController enemyController = rootobj.GetComponent<EnemyController>();
            enemyController.Finded();
        }
    }
}
