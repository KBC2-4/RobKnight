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


    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        // ゲームの時間を停止
        Time.timeScale = 0f;
        Debug.Log("ゲームオーバー！");
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelect);
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
}

