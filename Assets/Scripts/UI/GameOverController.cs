using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverPanel;
    //　最初にフォーカスするゲームオブジェクト
    [SerializeField]
    public GameObject firstSelect;
    [SerializeField] private AudioSource bgmAudioSource; // 音量を小さくするBGM再生に使用するオーディオソース
    [SerializeField] private AudioSource seAudioSource; // ループさせない為SE再生に使用するオーディオソース
    [SerializeField] private AudioClip bgm; // 再生するBGM


    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        // ゲームの時間を停止
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelect);
        // 音量を小さくする
        bgmAudioSource.volume = 0.5f;
        seAudioSource.volume = 0.5f;
        // SE再生
        seAudioSource.PlayOneShot(bgm);
    }

    public void RestartGame()
    {
        // ゲームの時間を初期状態で通常の速度に設定
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToTitle()
    {
        // ゲームの時間を初期状態で通常の速度に設定
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

