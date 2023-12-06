using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseController : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private GameObject pauseMenu; // ポーズメニューのUIをアタッチします
    [SerializeField] private GameObject defaultButton;
    [SerializeField] private Volume postProcessVolume;
    private DepthOfField _depthOfField;

    void Start()
    {
        // ゲームの時間を初期状態で通常の速度に設定
        Time.timeScale = 1f;
        // ポーズメニューのUIを非表示に設定
        pauseMenu.SetActive(false);

        // VolumeからDepth of Fieldコンポーネントを取得
        if (postProcessVolume.profile.TryGet<DepthOfField>(out _depthOfField))
        {
            // 初期設定（必要に応じて）
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
            // ゲームの時間を停止
            Time.timeScale = 0f;
            // ポーズメニューのUIを表示
            pauseMenu.SetActive(true);
            // デフォルトのボタンにを選択する
            EventSystem.current.SetSelectedGameObject(defaultButton);
            
            if (_depthOfField != null)
            {
                _depthOfField.active = true;
            }
        }
        else
        {
            // ゲームの時間を再開
            Time.timeScale = 1f;
            // ポーズメニューのUIを非表示
            pauseMenu.SetActive(false);
            
            if (_depthOfField != null)
            {
                _depthOfField.active = false;
            }
        }
    }
}
