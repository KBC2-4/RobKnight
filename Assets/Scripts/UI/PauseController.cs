using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseController : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private GameObject pauseMenu; // �|�[�Y���j���[��UI���A�^�b�`���܂�
    [SerializeField] private GameObject defaultButton;
    [SerializeField] private Volume postProcessVolume;
    private DepthOfField _depthOfField;
    private GameObject lastSelectedButton; // �Ō�ɑI�����ꂽ�{�^��
    [SerializeField] private GameObject _settingsPanel;
    // [SerializeField] private SettingsAnimatorController _settingsAnimatorController;


    void Start()
    {
        // �Q�[���̎��Ԃ�������ԂŒʏ�̑��x�ɐݒ�
        Time.timeScale = 1f;
        // �|�[�Y���j���[��UI���\���ɐݒ�
        pauseMenu.SetActive(false);

        // �ݒ��ʂ��A�N�e�B�u�ɐݒ�
        _settingsPanel.SetActive(false);

        // Volume����Depth of Field�R���|�[�l���g���擾
        if (postProcessVolume.profile.TryGet<DepthOfField>(out _depthOfField))
        {
            // �����ݒ�i�K�v�ɉ����āj
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    // <summary>
    // �u�ݒ�v�{�^������Ăяo����� �ݒ��ʂ��J��
    // </summary>
    public void ShowSettingsPanel()
    {
        _settingsPanel.SetActive(true);
    }

    // <summary>
    // �u�߂�v�{�^������Ăяo����� �ݒ��ʂ����
    // </summary>
    public void HideSettingsPanel()
    {
        _settingsPanel.SetActive(false);
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // �X�e�[�g��ύX
            UIManager.Instance.ChangeState(UIManager.UIState.PauseMenu);

            // �|�[�Y�O�̑I����ۑ�
            lastSelectedButton = EventSystem.current.currentSelectedGameObject;

            // �Q�[���̎��Ԃ��~
            Time.timeScale = 0f;
            // �|�[�Y���j���[��UI��\��
            pauseMenu.SetActive(true);
            // �f�t�H���g�̃{�^����I������
            EventSystem.current.SetSelectedGameObject(defaultButton);
            
            if (_depthOfField != null)
            {
                _depthOfField.active = true;
            }
        }
        else
        {
            // �X�e�[�g���ݒ��ʂ̏ꍇ�A�ݒ��ʂ����
            if (UIManager.Instance.GetCurrentState() == UIManager.UIState.SettingsMenu)
            {
                _settingsPanel.SetActive(false);
            }

            // �O��̃X�e�[�g�ɖ߂�

            // �Q�[����ʂ̃X�e�[�g�ɖ߂�
            UIManager.Instance.ChangeState(UIManager.UIState.Gameplay);

            if (lastSelectedButton != null)
            {
                // SE���Đ�����Ȃ��悤�ɂ���
                UISoundManager.Instance.SetProgrammaticSelect();
                // �|�[�Y�O�̑I���ɖ߂�
                EventSystem.current.SetSelectedGameObject(lastSelectedButton);
            }

            // �Q�[���̎��Ԃ��ĊJ
            Time.timeScale = 1f;
            // �|�[�Y���j���[��UI���\��
            pauseMenu.SetActive(false);
            if (_depthOfField != null)
            {
                _depthOfField.active = false;
            }
        }
    }
}
