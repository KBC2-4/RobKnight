using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        // ���C���J�������擾
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up);
            
            // ��ɃJ�����Ɠ��������ɐݒ�
            //transform.rotation = mainCamera.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Main Camera������܂���B\nMain Camera�^�O��ݒ肵�Ă�������");
        }
    }
}