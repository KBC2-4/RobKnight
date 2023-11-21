using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform playerTransform;
    public float despawnDistance = 10f;
    public GameObject objectToSpawn;
    public int maxSpawnedObjects = 5;   // スポーンさせるオブジェクトの最大数
    private List<GameObject> spawnedObjects = new List<GameObject>(); // スポーンされたオブジェクトを追跡するためのリスト
    private int currentSpawnedObjects = 0;  // スポーンさせたオブジェクトの数
    //public Vector3 areaSize = new Vector3(10, 0, 10); // スポーンエリアのサイズ
    //public Vector3 areaOffset; // スポーンエリアのオフセット
    private BoxCollider spawnArea;
    public float spawnDelay = 2.0f; // スポーンの遅延時間（秒）

    void Start()
    {
        spawnArea = GetComponent<BoxCollider>();
        StartCoroutine(SpawnRoutine()); // スポーンルーチンを開始
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // playerタグを持つオブジェクトのTransformを取得
    }

    void Update()
    {
        // if (currentSpawnedObjects < maxSpawnedObjects)
        // {
        //     SpawnObjectInRandomPosition();
        // }

        // エネミーのデスポーン
        //ObjectPoolManager.Instance.PerformActionOnActiveObjects(enemy =>
        //{
        //    if (Vector3.Distance(playerTransform.position, enemy.transform.position) > despawnDistance)
        //    {
        //        // プレイヤーから離れたエネミーをプールに戻す
        //        ObjectPoolManager.Instance.ReturnEnemy(enemy);
        //    }
        //});
        
        
        // スポーンされたエネミーのアクティブ状態をチェック
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i); // 既に破棄されたオブジェクトをリストから削除
            }
            else
            {
                float distanceToPlayer = Vector3.Distance(playerTransform.position, spawnedObjects[i].transform.position);

               
                //{
                //    // プレイヤーが離れたらオブジェクトを非アクティブ化
                //    spawnedObjects[i].SetActive(false);
                //}
                if (distanceToPlayer < despawnDistance)
                {
                    // プレイヤーが近づいたらオブジェクトをアクティブ化
                    spawnedObjects[i].SetActive(true);
                }
            }
        }
    }
    
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay); // 遅延

            if (spawnedObjects.Count < maxSpawnedObjects)
            {
                SpawnObjectInRandomPosition();
            }
        }
    }

    public void SpawnObjectInRandomPosition()
    {
        //Vector3 randomPosition = new Vector3(
        //    Random.Range(-areaSize.x / 2, areaSize.x / 2),
        //    Random.Range(-areaSize.y / 2, areaSize.y / 2),
        //    Random.Range(-areaSize.z / 2, areaSize.z / 2)
        //) + areaOffset;

        //Instantiate(objectToSpawn, transform.position + randomPosition, Quaternion.identity);
        //objectToSpawn = ObjectPoolManager.Instance.GetEnemy();
        Vector3 randomPoint = new Vector3(
            Random.Range(-spawnArea.size.x / 2, spawnArea.size.x / 2),
            Random.Range(-spawnArea.size.y / 2, spawnArea.size.y / 2),
            Random.Range(-spawnArea.size.z / 2, spawnArea.size.z / 2)
        ) + spawnArea.center;

        // エネミープールからエネミーをスポーン
        //EnemyController spawnedEnemy = enemyPoolManager.SpawnEnemy(spawnPosition, spawnRotation);
        // EnemyPoolManager のインスタンスを Singleton 経由で取得し、エネミーをスポーン
        //EnemyController spawnedEnemy = EnemyPoolManager.Instance.SpawnEnemy(transform.TransformPoint(randomPoint), Quaternion.identity);
        GameObject spawned = Instantiate(objectToSpawn, transform.TransformPoint(randomPoint), Quaternion.identity);
        spawnedObjects.Add(spawned); // スポーンされたオブジェクトをリストに追加
        currentSpawnedObjects++;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.transform == playerTransform)
        {
            // プレイヤーが範囲内に入ったら全てのエネミーをアクティブ化
            foreach (var enemy in spawnedObjects)
            {
                if (enemy != null)
                {
                    enemy.SetActive(true);
                }
            }
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.transform == playerTransform)
    //    {
    //        // プレイヤーが範囲外に出たら全てのエネミーを非アクティブ化
    //        foreach (var enemy in spawnedObjects)
    //        {
    //            if (enemy != null)
    //            {
    //                enemy.SetActive(false);
    //            }
    //        }
    //    }
    //}
}

