using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStoneStatue : MonoBehaviour
{


   void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform myTransform = this.transform;

        //�i���Ɖ�]���鏈��
        // transform���擾
        //// ���[���h���W��ŁA���݂̉�]�ʂ։��Z����
        //myTransform.Rotate(0, 1.0f, 0, Space.World);


        Vector3 localAngle = myTransform.localEulerAngles;
        localAngle.z = 15.0f; // ���[�J�����W����ɁAz�������ɂ�����]��10�x�ɕύX

        myTransform.localEulerAngles = localAngle; // ��]�p�x��ݒ�

    }
}
