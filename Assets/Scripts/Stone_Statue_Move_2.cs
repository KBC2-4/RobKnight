using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Statue_Move_2 : MonoBehaviour
{
    public button Button;

    public bool push_flg;
    public bool isAttacked; //�U����H������̂�
    int count; //������Ă���ԁ{����Ă���

    public float power = 1f;  //�����ꂽ�Ƃ��ɂ������

    // Start is called before the first frame update
    void Start()
    {
        push_flg = false;
        isAttacked = false;
    }

    private float forcetime = 0;                      //�����o������
    // Update is called once per frame
    void Update()
    {
        if (0 < forcetime)
        {
            transform.position += -transform.up * Time.deltaTime * forcetime * power;
            push_flg = false;
        }
        else
        {
            transform.Rotate(0f, 0f, 90 * Time.deltaTime); //��]
        }

        //�����o�����Ԃ����炷
        forcetime -= Time.deltaTime;
        if (forcetime < 0)
        {
            forcetime = 0;
        }

        transform.position = (new Vector3(Mathf.Clamp(transform.position.x, 347, 372), transform.position.y, Mathf.Clamp(transform.position.z, 707, 729)));
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
                    if (playerController.IsAttacking == true)
                    {
                        //������鎞�Ԃ�ݒ肷��
                        forcetime = 1;
                    }
                }
            }
        }
    }
}








