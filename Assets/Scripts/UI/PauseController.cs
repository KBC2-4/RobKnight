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

    void Start()
    {
        // �Q�[���̎��Ԃ�������ԂŒʏ�̑��x�ɐݒ�
        Time.timeScale = 1f;
        // �|�[�Y���j���[��UI���\���ɐݒ�
        pauseMenu.SetActive(false);

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

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // �Q�[���̎��Ԃ��~
            Time.timeScale = 0f;
            // �|�[�Y���j���[��UI��\��
            pauseMenu.SetActive(true);
            // �f�t�H���g�̃{�^���ɂ�I������
            EventSystem.current.SetSelectedGameObject(defaultButton);
            
            if (_depthOfField != null)
            {
                _depthOfField.active = true;
            }
        }
        else
        {
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
