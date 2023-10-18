using UnityEngine;

public class PauseController : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenu; // �|�[�Y���j���[��UI���A�^�b�`���܂�

    void Start()
    {
        // �Q�[���̎��Ԃ�������ԂŒʏ�̑��x�ɐݒ�
        Time.timeScale = 1f;
        // �|�[�Y���j���[��UI���\���ɐݒ�
        pauseMenu.SetActive(false);
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
        }
        else
        {
            // �Q�[���̎��Ԃ��ĊJ
            Time.timeScale = 1f;
            // �|�[�Y���j���[��UI���\��
            pauseMenu.SetActive(false); 
        }
    }
}
