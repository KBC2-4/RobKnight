using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    private bool isOpen = false;   // �󔠂��J���ꂽ���ǂ����̃t���O

    private void OnTriggerEnter(Collider other)
    {
        // ���̃I�u�W�F�N�g���g���K�[�R���C�_�[�ɐڐG�������̏���
        if (other.CompareTag("Player") && !isOpen)
        {
            OpenBox();
        }
    }

    // �󔠂��J���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    private void OpenBox()
    {
        isOpen = true; // �󔠂��J���ꂽ���Ƃ��t���O�Ŏ���
    }

}
