using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public GameObject enemyPrefab; // �G�l�~�[�̃v���n�u���Q��
    public int poolSize = 10; // �v�[���̃T�C�Y��ݒ�

    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���J�ڂŔj������Ȃ��悤�ɂ���
            InitializePool(); // �v�[���̏�����
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // ���ɃC���X�^���X�����݂���ꍇ�͔j������
        }

    }

    private void InitializePool()
    {
        // �G�l�~�[�v�[���̏�����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    // �v�[������G�l�~�[���擾
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
            // �v�[������̏ꍇ�A�V�����G�l�~�[��ǉ�
            GameObject enemy = Instantiate(enemyPrefab);
            return enemy;
        }
    }

    // �G�l�~�[���v�[���ɕԋp
    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }

    // �v�[�����̃I�u�W�F�N�g�̐���Geeter
    public int GetPoolCount()
    {
        return enemyPool.Count;
    }

    // �v�[�����̃A�N�e�B�u�ȃI�u�W�F�N�g�ɑ΂��ăA�N�V���������s����
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

