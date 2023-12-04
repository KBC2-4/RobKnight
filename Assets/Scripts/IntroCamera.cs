using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCamera : MonoBehaviour
{
    [SerializeField] GameObject _introCamera; // �C���g���J�����I�u�W�F�N�g
    [SerializeField] Transform _spawnPoint; // �v���C���[�̃X�|�[���ʒu
    private GameObject _mainCamera; // ���C���J����
    private GameObject _player; // �v���C���[�I�u�W�F�N�g
    private Animator _introAnimator;    // �C���g���J�����̃A�j���[�^�[

    public event Action OnIntroAnimationComplete;   //  �C���g���A�j���[�V�������I�������Ƃ��ɔ��s�����C�x���g

    void Start()
    {
        // �^�O���g���Ċe�I�u�W�F�N�g�������擾
        _player = GameObject.FindWithTag("Player");
        _mainCamera = GameObject.FindWithTag("MainCamera");

        // �Q�[���J�n���ɃC���g���J�����ɐ؂�ւ���
        CameraManager.Instance.SwitchCamera(_introCamera);

        // �C���g���J�����̃A�j���[�^�[���擾
        _introAnimator = _introCamera.GetComponent<Animator>();

        // �v���C���[���\��
        // _player.SetActive(false);

        // StartCoroutine(ShowStageIntro());
    }

        void Update()
    {
        // �A�j���[�V�������I���������ǂ������`�F�b�N
        if (_introAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            OnIntroAnimationComplete?.Invoke();
            CameraManager.Instance.SwitchToMainCamera();
        }


        // �f�o�b�O�p
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            _introAnimator.speed++;
        }
#endif
    }

    IEnumerator ShowStageIntro()
    {
        // �C���g���V�[����\�����邽�߂ɐ��b�҂�
        yield return new WaitForSeconds(5);

        // ���C���J�����ɐ؂�ւ���
        CameraManager.Instance.SwitchCamera(_mainCamera);


        // �v���C���[��\��
        // _player.SetActive(true);
        // �v���C���[���X�|�[���ʒu�ɔz�u����
        _player.transform.position = _spawnPoint.position;
    }

    // �A�j���[�V�����C�x���g����Ăяo����郁�\�b�h
    public void OnIntroAnimationEnd()
    {
        // ���C���J�����ɐ؂�ւ���
        CameraManager.Instance.SwitchCamera(_mainCamera);

        // �v���C���[��\�����A�X�|�[���ʒu�ɔz�u
        // _player.SetActive(true);
        _player.transform.position = _spawnPoint.position;
    }
}
