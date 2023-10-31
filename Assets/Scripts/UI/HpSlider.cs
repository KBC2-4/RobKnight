using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HpSlider : MonoBehaviour
{
    private EnemyController enemyController;
    private Slider hpSlider;

    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
        hpSlider = GetComponentInChildren<Slider>();
        hpSlider.maxValue = enemyController.enemyData.maxHp;
        hpSlider.value = enemyController.enemyData.hp;
    }

    void Update()
    {
        hpSlider.value = enemyController.enemyData.hp;
    }

    void LateUpdate()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // ��ɃJ�����Ɠ��������ɐݒ�
            transform.rotation = mainCamera.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Main Camera������܂���B\nMain Camera�^�O��ݒ肵�Ă�������");
        }
    }

}
