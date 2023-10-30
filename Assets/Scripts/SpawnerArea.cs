using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerArea : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform spawnPoint;

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーがトリガーエリアに入った時
        if (other.tag == "Player")
        {
            SpawnObject();
        }
    }

    public void SpawnObject()
    {
        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}