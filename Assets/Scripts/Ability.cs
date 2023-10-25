using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Ability", order = 51)]
public class Ability : ScriptableObject
{
    public string abilityName;
    public int damage;
    // �Z�̍Ďg�p�܂ł̎���
    public float cooldown;
    // �Z�̃G�t�F�N�g
    public GameObject effectPrefab;

    public void Use(Transform user)
    {
        // �����ŋZ�̌��ʂ���������
        Debug.Log($"{user.name} �� {abilityName} ���g����!");
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, user.position, user.rotation);
        }
    }
}

