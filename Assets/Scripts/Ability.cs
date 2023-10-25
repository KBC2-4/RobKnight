using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Ability", order = 51)]
public class Ability : ScriptableObject
{
    public string abilityName;
    public int damage;
    // 技の再使用までの時間
    public float cooldown;
    // 技のエフェクト
    public GameObject effectPrefab;

    public void Use(Transform user)
    {
        // ここで技の効果を実装する
        Debug.Log($"{user.name} は {abilityName} を使った!");
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, user.position, user.rotation);
        }
    }
}

