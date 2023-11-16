using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform playerTransform;
    public float despawnDistance = 10f;
    public GameObject objectToSpawn;
    public int maxSpawnedObjects = 5;   // �X�|�[��������I�u�W�F�N�g�̍ő吔
    private List<GameObject> spawnedObjects = new List<GameObject>(); // �X�|�[�����ꂽ�I�u�W�F�N�g��ǐՂ��邽�߂̃��X�g
    private int currentSpawnedObjects = 0;  // �X�|�[���������I�u�W�F�N�g�̐�
    //public Vector3 areaSize = new Vector3(10, 0, 10); // �X�|�[���G���A�̃T�C�Y
    //public Vector3 areaOffset; // �X�|�[���G���A�̃I�t�Z�b�g
    private BoxCollider spawnArea;
    public float spawnDelay = 2.0f; // �X�|�[���̒x�����ԁi�b�j

    void Start()
    {
        spawnArea = GetComponent<BoxCollider>();
        StartCoroutine(SpawnRoutine()); // �X�|�[�����[�`�����J�n
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // player�^�O�����I�u�W�F�N�g��Transform���擾
    }

    void Update()
    {
        // if (currentSpawnedObjects < maxSpawnedObjects)
        // {
        //     SpawnObjectInRandomPosition();
        // }

        // �G�l�~�[�̃f�X�|�[��
        //ObjectPoolManager.Instance.PerformActionOnActiveObjects(enemy =>
        //{
        //    if (Vector3.Distance(playerTransform.position, enemy.transform.position) > despawnDistance)
        //    {
        //        // �v���C���[���痣�ꂽ�G�l�~�[���v�[���ɖ߂�
        //        ObjectPoolManager.Instance.ReturnEnemy(enemy);
        //    }
        //});
        
        
        // �X�|�[�����ꂽ�G�l�~�[�̃A�N�e�B�u��Ԃ��`�F�b�N
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i); // ���ɔj�����ꂽ�I�u�W�F�N�g�����X�g����폜
            }
            else
            {
                float distanceToPlayer = Vector3.Distance(playerTransform.position, spawnedObjects[i].transform.position);

               
                //{
                //    // �v���C���[�����ꂽ��I�u�W�F�N�g���A�N�e�B�u��
                //    spawnedObjects[i].SetActive(false);
                //}
                if (distanceToPlayer < despawnDistance)
                {
                    // �v���C���[���߂Â�����I�u�W�F�N�g���A�N�e�B�u��
                    spawnedObjects[i].SetActive(true);
                }
            }
        }
    }
    
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay); // �x��

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

        // �G�l�~�[�v�[������G�l�~�[���X�|�[��
        //EnemyController spawnedEnemy = enemyPoolManager.SpawnEnemy(spawnPosition, spawnRotation);
        // EnemyPoolManager �̃C���X�^���X�� Singleton �o�R�Ŏ擾���A�G�l�~�[���X�|�[��
        //EnemyController spawnedEnemy = EnemyPoolManager.Instance.SpawnEnemy(transform.TransformPoint(randomPoint), Quaternion.identity);
        GameObject spawned = Instantiate(objectToSpawn, transform.TransformPoint(randomPoint), Quaternion.identity);
        spawnedObjects.Add(spawned); // �X�|�[�����ꂽ�I�u�W�F�N�g�����X�g�ɒǉ�
        currentSpawnedObjects++;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.transform == playerTransform)
        {
            // �v���C���[���͈͓��ɓ�������S�ẴG�l�~�[���A�N�e�B�u��
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
    //        // �v���C���[���͈͊O�ɏo����S�ẴG�l�~�[���A�N�e�B�u��
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

