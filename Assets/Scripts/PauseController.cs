using UnityEngine;

public class PauseController : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenu; // ポーズメニューのUIをアタッチします

    void Start()
    {
        // ゲームの時間を初期状態で通常の速度に設定
        Time.timeScale = 1f;
        // ポーズメニューのUIを非表示に設定
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
            // ゲームの時間を停止
            Time.timeScale = 0f;
            // ポーズメニューのUIを表示
            pauseMenu.SetActive(true);
        }
        else
        {
            // ゲームの時間を再開
            Time.timeScale = 1f;
            // ポーズメニューのUIを非表示
            pauseMenu.SetActive(false); 
        }
    }
}
