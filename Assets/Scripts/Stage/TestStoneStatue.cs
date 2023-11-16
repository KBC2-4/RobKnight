using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStoneStatue : MonoBehaviour
{
    /*
     * �e�ϐ��̐錾�̎d��
    [Serializable] Int32 a = 3;�@�@private public�̒���
    private Int32 _b = 4;
    public Int32 c = 5;
    */

    public bool isAttacked = false; //�U����H������̂�
    public bool during_rotation = false; //��]���Ȃ̂�

    private float _old_rotaey = 0;


    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    public float pushPower = 2.0F;

    void OnTriggerStay(Collider other)
    {
        // �Փ˂����I�u�W�F�N�g���v���C���[�̏ꍇ
        if (other.gameObject.CompareTag("Player"))
        {
            // �v���C���[�R���g���[���[���擾
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();    

            // �v���C���[���߈˂��Ă����Ԃ��m�F���܂��B
            if (playerController != null)
            {
                if (playerController.PossessionEnemyName == "Gobrin")
                {
                    if (playerController.IsAttacking == true && during_rotation == false)
                    {
                        _old_rotaey = transform.localEulerAngles.y;
                        during_rotation = true;
                        isAttacked = false;
                    }

                    if (during_rotation)
                    {
                        if (_old_rotaey + 15 >= transform.localEulerAngles.y)
                        {
                            transform.Rotate(0f, 0f, 10 * Time.deltaTime); //��]
                        }
                        else
                        {
                            during_rotation = false;
                        }
                    }
                }
            }
        }
    }
}