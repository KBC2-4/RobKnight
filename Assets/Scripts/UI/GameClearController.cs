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
    private float _timeRemainingF = 15f; // ���ڂ܂ł̎���(�b)
    [SerializeField] GameObject _canvas; // �Q�[���N���ACanvas
    [SerializeField] GameObject _camera; // �{�X�p�̃J����
    private CameraController _cameraController; // �J�����ɃA�^�b�`����Ă���R���g���[���[�X�N���v�g
    private bool hasExecuted = false;   // 1��̂ݎ��s�����t���O
    [SerializeField] private PlayerController _playerController;

    void Awake()
    {

    }

    void Start()
    {
        _canvas.SetActive(false);
        _camera.SetActive(false);
        _cameraController = _camera.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_boss.isDeath)
        {
            if (!hasExecuted)
            {
                // �v���C���[����𖳌�������
                _playerController.SetInputAction(false);

                // �v���C���[�𖳓G�A�s�����[�h�ɂ���
                // ??????????????????????????

                _camera.SetActive(true);

                // �X���[���[�V�����ɂ���
                Time.timeScale = 0.5f;

                // �J�������{�X�𒆐S�ɉ�]������
                _cameraController.StartRotation();

                // �Q�[���N���A��ʂ�\��
                _canvas.SetActive(true);

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