using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyInfo", menuName = "Enemy/Enemy Info", order = 52)]
public class EnemyInfo : ScriptableObject
{
    public string name;    // –¼‘O
    public string displayName;  // •\¦–¼–¼‘O
    public Sprite image;   // ‰æ‘œ
    public string description; // à–¾
}

