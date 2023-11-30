using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Statue_Move : MonoBehaviour
{

    public bool push_flg;
    public bool isAttacked = false; //�U����H������̂�
    public bool during_rotation = false; //��]���Ȃ̂�


    // Start is called before the first frame update
    void Start()
    {
        push_flg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (push_flg == true)
        {
            transform.position += -transform.up * Time.deltaTime;
            push_flg = false;
        }
        if (during_rotation == true)
        {
            transform.Rotate(0f, 0f, 10 * Time.deltaTime); //��]
        }
    }

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
                    push_flg = true;
                    if (playerController.IsAttacking == true )
                    {
                        during_rotation = true;
                        isAttacked = false;
                    }


                }
            }
        }
    }
}
