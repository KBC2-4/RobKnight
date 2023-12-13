using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Tooltip("�v���C���[��Transform")] private Transform _playerTransform; // �v���C���[��Transform
    [SerializeField, Tooltip("�{�X��Transform")] private Transform _bossTransform; // �{�X��Transform
    [SerializeField, Tooltip("��]���x")] private float _rotationSpeed = 50f; // ��]���x

    void Awake()
    {
        if (_playerTransform == null)
        {
            _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        }
    }

    public void StartRotation()
    {
        StartCoroutine(RotateAroundBoss());
    }

    IEnumerator RotateAroundBoss()
    {
        // �J�����̏����ʒu��ݒ�
        Vector3 bossPosition = _bossTransform.position;
        Vector3 cameraPosition = bossPosition + new Vector3(0, 5, -10); // �{�X�̏�����ƌ��ɔz�u
        transform.position = cameraPosition;

        while (true) // �������[�v
        {
            // �{�X�̕�������
            transform.LookAt(_bossTransform);
            // �{�X�𒆐S�ɉ�]������
            transform.RotateAround(_bossTransform.position, Vector3.up, _rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
