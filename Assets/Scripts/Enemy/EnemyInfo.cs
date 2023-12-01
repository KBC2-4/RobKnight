using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyInfo", menuName = "Enemy/Enemy Info", order = 52)]
public class EnemyInfo : ScriptableObject
{
    public string name;    // ���O
    public string displayName;  // �\�������O
    public Sprite image;   // �摜
    public string description; // ����
}

