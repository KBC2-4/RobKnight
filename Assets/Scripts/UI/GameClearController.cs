using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameClearController : MonoBehaviour
{
    [SerializeField] private EnemyController _boss;
    [SerializeField] private TMP_Text _countdownText; // UI�e�L�X�g�ւ̎Q��
    private static float _timeRemainingF = 30f;
    [SerializeField] private GameObject _canvas; // �Q�[���N���ACanvas
    [SerializeField] private GameObject _camera; // �{�X�p�̃J����
    [SerializeField] private GameObject _cameraParent; // �{�X�p�̃J�����̐e�I�u�W�F�N�g
    private CameraController _cameraController; // �J�����ɃA�^�b�`����Ă���R���g���[���[�X�N���v�g
    [SerializeField] private GameObject _cameraAnimatorController; // �{�X�p�̃J�����̐e�I�u�W�F�N�g���Ǘ����Ă���R���g���[���[
    private CameraAnimatorController _cameraAnimatorControllerS; // �J�����̐e�I�u�W�F�N�g�ɃA�^�b�`����Ă���R���g���[���[�X�N���v�g
    private bool hasExecuted = false;   // 1��̂ݎ��s�����t���O
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private AudioSource _bgmAudioSource; // BGM�Đ��Ɏg�p����I�[�f�B�I�\�[�X
    [SerializeField] private AudioClip _bgmClip; // �Đ�����BGM

    void Awake()
    {
        if(_playerController == null)
        {
            // _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            _playerController = FindObjectOfType<PlayerController>();
        }

        if (_cameraAnimatorController != null)
        {
            _cameraAnimatorControllerS = _cameraAnimatorController.GetComponent<CameraAnimatorController>();
        }
        
    }

    private void OnEnable()
    {
        if (_cameraAnimatorController != null)
        {
            _cameraAnimatorControllerS.OnAnimationComplete += HandleAnimationComplete;
        }
    }

    private void OnDisable()
    {

        if (_cameraAnimatorController != null)
        {
            _cameraAnimatorControllerS.OnAnimationComplete -= HandleAnimationComplete;
        }
    }

    /// <summary>
    /// �A�j���[�V���������������ۂɌĂяo�����n���h���[�B���̊֐��ł�UI��\����Ԃɐ؂�ւ��܂��B
    /// �A�j���[�V�����̊��������m���āAUI�\���t���O��true�ɐݒ肵�A�K�C�h�L�����o�X���A�N�e�B�u�ɂ��܂��B
    /// </summary>
    private void HandleAnimationComplete()
    {
        // �ʏ�̑��x�ɖ߂�
        Time.timeScale = 1f;

        // �V�����N���b�v��ݒ�
        _bgmAudioSource.clip = _bgmClip;
        // �V�����N���b�v���Đ��J�n
        _bgmAudioSource.Play();

        // �A�j���[�V�����������UI��\��
        // �Q�[���N���A��ʂ�\��
        _canvas.SetActive(true);
    }

    void Start()
    {
        _canvas.SetActive(false);
        // _camera.SetActive(false);

        if(_cameraController != null)
        {
            _cameraController = _camera.GetComponent<CameraController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_boss.isDeath)
        {
            if (!hasExecuted)
            {
                
                if (_playerController != null)
                {
                    // �v���C���[����𖳌�������
                    // _playerController.SetInputAction(false);
                    // �v���C���[�𖳓G�ɂ���
                    _playerController.IsInfinity = true;
                }

                // �K�C�h�o�[���\���ɂ���
                GuideBarController.Instance.SetUIVisibility(false);

                _camera.SetActive(true);

                // �X���[���[�V�����ɂ���
                Time.timeScale = 0.3f;

                // �J�������{�X�𒆐S�ɉ�]������
                // _cameraController.StartRotation();
                _cameraAnimatorControllerS.StartRotation();

                //// �Q�[���N���A��ʂ�\��
                //_canvas.SetActive(true);

                // �t���O���X�V
                hasExecuted = true;
            }

            // ���b��ɉ�ʑJ�ځi�^�C�g���֐��ځj
            if (_timeRemainingF > 1)
            {
                // �o�ߎ��Ԃ��J�E���g
                _timeRemainingF -= Time.deltaTime;
                // _countdownText.text = Mathf.Round(_timeRemainingF).ToString();
                _countdownText.text = _timeRemainingF.ToString("F0") + "�b��Ƀ^�C�g���֖߂�܂�";

            }
            else
            {

                _countdownText.text = "�^�C�g���֖߂�܂�...";
                _camera.SetActive(false);
                SceneManager.LoadScene("Title");
            }
        }
    }

    void OnDestroy()
    {
        hasExecuted = false;


    }
}