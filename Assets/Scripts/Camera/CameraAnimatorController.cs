using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAnimatorController : MonoBehaviour
{
    [SerializeField] private GameObject _cameraParent;  // �J�����̐e�I�u�W�F�N�g
    [SerializeField, Tooltip("�{�X��Transform")] private Transform _bossTransform; // �{�X��Transform
    [SerializeField, Tooltip("��]���x")] private float _rotationSpeed = 30f; // ��]���x
    private Vector3 _startPos;
    private Animator _animator;
    private bool _isAnimating = true; // �A�j���[�V���������ǂ�����ǐՂ���t���O
    public event Action OnAnimationComplete;   //  �A�j���[�V�������I�������Ƃ��ɔ��s�����C�x���g

    private void Awake()
    {
        if (_cameraParent != null)
        {

            _startPos = _cameraParent.transform.position;
            _cameraParent.SetActive(false);
            _animator = _cameraParent.GetComponent<Animator>();
        }
    }

    public void StartRotation()
    {
        if (_cameraParent != null)
        {
            _cameraParent.SetActive(true);
        }
        // StartCoroutine(RotateAroundBoss());
    }

    IEnumerator RotateAroundBoss()
    {
        // �J�����̏����ʒu��ݒ�
        //Vector3 bossPosition = _bossTransform.position;
        //Vector3 cameraPosition = bossPosition + new Vector3(0, 5, -10); // �{�X�̏�����ƌ��ɔz�u
        //transform.position = cameraPosition;

        while (true) // �������[�v
        {
            // �{�X�̕�������
            _cameraParent.transform.GetChild(0).transform.LookAt(_bossTransform);
            // �{�X�𒆐S�ɉ�]������
            _cameraParent.transform.GetChild(0).transform.RotateAround(_bossTransform.position, Vector3.up, _rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }


public void Animate()
    {
        _cameraParent.transform.position = _startPos;
    }

    public void Animate(Vector3 pos)
    {
        _cameraParent.transform.position = pos;
    }

    public void Animate(Vector3 pos, float time)
    {
        _cameraParent.transform.position = Vector3.Lerp(_startPos, pos, time);
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 bossPosition = _bossTransform.position;
        Vector3 cameraPosition = bossPosition + new Vector3(0, 1.8f, -6.1111f); // �{�X�̏�����ƌ��ɔz�u
        _cameraParent.transform.position = cameraPosition;
    }

    private void Update()
    {
        // �A�j���[�V�������I���������ǂ������`�F�b�N
        if (_isAnimating && !_animator.IsInTransition(0) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            _isAnimating = false;
            _animator.enabled = false; // Animator�𖳌���
            OnAnimationComplete?.Invoke();
            StartCoroutine(RotateAroundBoss());
        }
    }
}
