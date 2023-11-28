using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyInfo", menuName = "Enemy/Enemy Info", order = 52)]
public class EnemyInfo : ScriptableObject
{
    public string name;    // 名前
    public Sprite image;   // 画像
    public string description; // 説明
}

