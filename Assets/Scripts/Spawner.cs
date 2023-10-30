using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform[] spawnPoints;
    public bool IsRandomly = true;

    void Start()
    {
        if (IsRandomly)
        {
            SpawnObjectRandomly();
        }
        else
        {
            SpawnObject();
        }
    }

    public void SpawnObject()
    {
        Instantiate(objectToSpawn, spawnPoints[0].position, spawnPoints[0].rotation);
    }

    public void SpawnObjectRandomly()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(objectToSpawn, spawnPoints[randomIndex].position, spawnPoints[randomIndex].rotation);
    }
}

