using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpSlider : MonoBehaviour
{

    void LateUpdate()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // ��ɃJ�����Ɠ��������ɐݒ�
            transform.rotation = mainCamera.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Main Camera������܂���B\nMain Camera�^�O��ݒ肵�Ă�������");
        }
    }

}
