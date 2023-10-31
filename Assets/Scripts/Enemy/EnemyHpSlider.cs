using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpSlider : MonoBehaviour
{
    public Slider hpSlider;
    public EnemyController enemyController;


    // Start is called before the first frame update
    void Start()
    {
        hpSlider = GetComponent<Slider>();
        if (hpSlider != null)
        {
            // スライダーの最大値と現在値を設定
            hpSlider.value = enemyController.enemyData.hp;
            hpSlider.maxValue = enemyController.enemyData.maxHp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = enemyController.enemyData.hp;
    }
}
