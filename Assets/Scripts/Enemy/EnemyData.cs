using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data", order = 51)]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int hp;
    public int maxHp;
    public int attackPower;
    public GameObject modelPrefab;
    // ‹Z
    public Ability[] abilities;


    public EnemyData(string name, int health, int maxHealth, int attack, GameObject model)
    {
        enemyName = name;
        hp = health;
        maxHp = maxHealth;
        attackPower = attack;
        modelPrefab = model;
    }
}

