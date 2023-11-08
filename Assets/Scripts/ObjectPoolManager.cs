using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public GameObject enemyPrefab; // エネミーのプレハブを参照
    public int poolSize = 10; // プールのサイズを設定

    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移で破棄されないようにする
            InitializePool(); // プールの初期化
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // 既にインスタンスが存在する場合は破棄する
        }

    }

    private void InitializePool()
    {
        // エネミープールの初期化
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    // プールからエネミーを取得
    public GameObject GetEnemy()
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            // プールが空の場合、新しいエネミーを追加
            GameObject enemy = Instantiate(enemyPrefab);
            return enemy;
        }
    }

    // エネミーをプールに返却
    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }

    // プール内のオブジェクトの数のGeeter
    public int GetPoolCount()
    {
        return enemyPool.Count;
    }

    // プール内のアクティブなオブジェクトに対してアクションを実行する
    public void PerformActionOnActiveObjects(Action<GameObject> action)
    {
        foreach (var enemy in enemyPool)
        {
            if (enemy.activeInHierarchy)
            {
                action(enemy);
            }
        }
    }


}

