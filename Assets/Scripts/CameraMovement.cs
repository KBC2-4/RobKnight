using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    /// <summary>
    /// �v���C���[��Transform
    /// </summary>
    [SerializeField]
    [Tooltip("�Ǐ]���������^�[�Q�b�g")]
    private GameObject target;

    /// <summary>
    /// �v���C���[�ƃJ�����̑��Έʒu
    /// </summary>
    public Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        //�J�����̈ʒu��ݒ�
        Vector3 position = target.transform.position;
        position.y += 10;
        gameObject.transform.position = position;
        
        //�Q�[���J�n���ɃJ�����ƃ^�[�Q�b�g�̋������擾
        offset = gameObject.transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�̈ʒu�ɃJ������Ǐ]������
        gameObject.transform.position = target.transform.position + offset;

        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
        //transform.Translate(movement);
    }
}
