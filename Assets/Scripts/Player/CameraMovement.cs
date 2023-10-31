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
    /// �^�[�Q�b�g�ƃJ�����̑��Έʒu
    /// </summary>
    public Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        //�J�����̈ʒu��ݒ�
        Vector3 position = target.transform.position;
        position.z -= 5;
        position.y += 10;
        gameObject.transform.position = position;


        //�Q�[���J�n���ɃJ�����ƃ^�[�Q�b�g�̋������擾
        offset = gameObject.transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //�^�[�Q�b�g�̈ʒu�ɃJ������Ǐ]������
        gameObject.transform.position = target.transform.position + offset;
    }

    /// <summary>
    /// �J�����̃^�[�Q�b�g��؂�ւ���
    /// </summary>
    /// <param name="newTarget">�V���Ƀ^�[�Q�b�g�ɂ������I�u�W�F�N�g</param>
    public void SetCameraTarget(GameObject newTarget)
    {
        target = newTarget;

        //�J�����̈ʒu��ݒ�
        Vector3 position = target.transform.position;
        position.z -= 5;
        position.y += 10;
        gameObject.transform.position = position;

        //�J�����ƃ^�[�Q�b�g�̋������擾
        offset = gameObject.transform.position - target.transform.position;
    }
}
