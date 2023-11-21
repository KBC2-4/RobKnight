using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneStatueNo2 : MonoBehaviour
{
    public bool isAttacked = false; //�U����H������̂�
    public bool during_rotation = false; //��]���Ȃ̂�

    private float _old_rotaey = 0;

    private float _fixation = 230; //�w�肵���p�x�ɂȂ������]���~

    public bool end = false; //�I�u�W�F�N�g�I��

    public GameObject obj;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

        if (_fixation > transform.localEulerAngles.y)
        {


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
        else
        {
            if (end == false)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);

                }
                end = true;
            }

        }


    }

    // Update is called once per frame

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
                }
            }
        }
    }
}