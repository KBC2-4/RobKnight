using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform bossTransform; // �{�X��Transform
    [SerializeField] private float rotationSpeed = 1f; // ��]���x

    public void StartRotation()
    {
        StartCoroutine(RotateAroundBoss());
    }

    IEnumerator RotateAroundBoss()
    {
        // �J�����̏����ʒu��ݒ�
        Vector3 bossPosition = bossTransform.position;
        Vector3 cameraPosition = bossPosition + new Vector3(0, 5, -10); // �{�X�̏�����ƌ��ɔz�u
        transform.position = cameraPosition;

        while (true) // �������[�v
        {
            // �{�X�̕�������
            transform.LookAt(bossTransform);
            // �{�X�𒆐S�ɉ�]������
            transform.RotateAround(bossTransform.position, Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
