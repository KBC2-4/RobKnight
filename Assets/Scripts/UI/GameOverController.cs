using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverPanel;


    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        // �Q�[���̎��Ԃ��~
        Time.timeScale = 0f;
        Debug.Log("�Q�[���I�[�o�[�I");
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        // �Q�[���̎��Ԃ�������ԂŒʏ�̑��x�ɐݒ�
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToTitle()
    {
        // �Q�[���̎��Ԃ�������ԂŒʏ�̑��x�ɐݒ�
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }
}

