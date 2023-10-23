using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data", order = 51)]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int hp;
    public int attackPower;
    public GameObject modelPrefab;

    public EnemyData(string name, int health, int attack, GameObject model)
    {
        enemyName = name;
        hp = health;
        attackPower = attack;
        modelPrefab = model;
    }
}

