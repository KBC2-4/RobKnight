using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneStatueMove : MonoBehaviour
{
    public button Button;
    public float power = 0.5f;

    public bool push_flg;
    public bool isAttacked = false; //�U����H������̂�

    [SerializeField]private AudioSource _audioSorce; // �Đ�����SE
    //int count; //������Ă���ԁ{����Ă���

    // Start is called before the first frame update
    void Start()
    {
        push_flg = false;
        _audioSorce = GetComponent<AudioSource>();
    }

    private Vector3 force = new Vector3(0, 0, 0);   //�����o����
    private Vector3 forcedecay = new Vector3(0, 0, 0);   //�����o���͂̌���
    private float forcetime = 0;                      //�����o������
    // Update is called once per frame
    void Update()
    {

       transform.position += force;

        //�����o�����ԂƗ͂����炷
        forcetime -= Time.fixedDeltaTime;
        if (forcetime < 0 || push_flg)
        {
            force = Vector3.zero;
            forcedecay = Vector3.zero;
            forcetime = 0;
            _audioSorce.Stop();
        }
        else
        {
            force -= (forcedecay * Time.fixedDeltaTime);
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
                if (playerController.PossessionEnemyName == "BigGoblin")
                {
                    //count++;
                    if (playerController.IsAttacking == true)
                    {
                        //playerController.transform.rotation = Quaternion.identity

                        Vector3 PlayerPos = playerController.transform.position;
                        Vector3 NowPos = transform.position;

                        PlayerPos.y = 0;
                        NowPos.y = 0;

                        //�v���C���[�ƑΏۊԂ̊p�x�����
                        var diff = (NowPos - PlayerPos).normalized;
                        Vector3 PushAngle = diff * power;

                        force = PushAngle;
                        forcedecay = PushAngle;
                        forcetime = 1;

                        isAttacked = false;
                        _audioSorce.Play();



                        //count = 0;
                    }

                    //if (count >= 5)
                    //{
                    //    push_flg = true;
                    //}
                }
            }
        }
    }
}