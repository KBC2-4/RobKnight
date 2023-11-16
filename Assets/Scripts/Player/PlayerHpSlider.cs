using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpSlider : MonoBehaviour
{
    public Slider hpSlider;
    public PlayerController playerMovement;


    // Start is called before the first frame update
    void Start()
    {
        Slider hpSlider = GetComponent<Slider>();
        if (hpSlider != null)
        {
            // �X���C�_�[�̍ő�l�ƌ��ݒl��ݒ�
            hpSlider.maxValue = playerMovement.hp;
            hpSlider.value = playerMovement.maxHp;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        hpSlider.value = playerMovement.hp;
    }
}
