using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{

    public bool push_flg;
    public bool event_flg; //����ɒ����G���A�̐Α������点�邩�̊m�F������
    float start_pds_y;


    // Start is called before the first frame update
    void Start()
    {
        push_flg = false;
        event_flg = false;
        start_pds_y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (push_flg == true)
        {
            transform.position += -transform.up * Time.deltaTime;
            if (start_pds_y - 0.1 >= transform.position.y) {
                push_flg = false;
                event_flg = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // �Փ˂����I�u�W�F�N�g���v���C���[�̏ꍇ
        if (other.gameObject.CompareTag("StoneStatue_area1"))
        {
            push_flg = true;
        }
        
    }
}