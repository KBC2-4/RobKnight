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
    [SerializeField] private AudioSource bgmAudioSource; // ���ʂ�����������BGM�Đ��Ɏg�p����I�[�f�B�I�\�[�X
    [SerializeField] private AudioSource seAudioSource; // ���[�v�����Ȃ���SE�Đ��Ɏg�p����I�[�f�B�I�\�[�X
    [SerializeField] private AudioClip bgm; // �Đ�����BGM


    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        // �Q�[���̎��Ԃ��~
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelect);
        // ���ʂ�����������
        bgmAudioSource.volume = 0.5f;
        seAudioSource.volume = 0.5f;
        // SE�Đ�
        seAudioSource.PlayOneShot(bgm);
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

    private void OnDestroy()
    {
        bgmAudioSource.volume = 1f;
        seAudioSource.volume = 1f;
        seAudioSource.Stop();
    }
}

