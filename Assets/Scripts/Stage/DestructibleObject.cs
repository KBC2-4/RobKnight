using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // �Փ˂����I�u�W�F�N�g���v���C���[�̏ꍇ
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[�R���g���[���[���擾
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            Debug.Log("�v���C���[���j�󂵂悤�Ƃ��Ă��܂�");

            // �v���C���[���߈˂��Ă����Ԃ��m�F���܂��B
            if (playerController != null && playerController.isPossession)
            {
                if (playerController.PossessionEnemyName == "Gobrin")
                {
                    // �߈˂��Ă���G�l�~�[���S�u�����̏ꍇ�I�u�W�F�N�g��j��
                    Destroy(gameObject);
                }
            }
        }
    }
}
