using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverPanel;
    //�@�ŏ��Ƀt�H�[�J�X����Q�[���I�u�W�F�N�g
    [SerializeField]
    public GameObject firstSelect;


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
        EventSystem.current.SetSelectedGameObject(firstSelect);
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

