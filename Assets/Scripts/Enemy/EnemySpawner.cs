using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform playerTransform;
    public float despawnDistance = 20f;
    public GameObject objectToSpawn;
    public int maxSpawnedObjects = 5;   // �X�|�[��������I�u�W�F�N�g�̍ő吔
    private int currentSpawnedObjects = 0;  // �X�|�[���������I�u�W�F�N�g�̐�
    //public Vector3 areaSize = new Vector3(10, 0, 10); // �X�|�[���G���A�̃T�C�Y
    //public Vector3 areaOffset; // �X�|�[���G���A�̃I�t�Z�b�g
    private BoxCollider spawnArea;
    public float spawnDelay = 2.0f; // �X�|�[���̒x�����ԁi�b�j

    void Start()
    {
        spawnArea = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (currentSpawnedObjects < maxSpawnedObjects)
        {
            SpawnObjectInRandomPosition();
        }

        // �G�l�~�[�̃f�X�|�[��
        //ObjectPoolManager.Instance.PerformActionOnActiveObjects(enemy =>
        //{
        //    if (Vector3.Distance(playerTransform.position, enemy.transform.position) > despawnDistance)
        //    {
        //        // �v���C���[���痣�ꂽ�G�l�~�[���v�[���ɖ߂�
        //        ObjectPoolManager.Instance.ReturnEnemy(enemy);
        //    }
        //});
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

        Instantiate(objectToSpawn, transform.TransformPoint(randomPoint), Quaternion.identity);

        currentSpawnedObjects++;
    }
}

