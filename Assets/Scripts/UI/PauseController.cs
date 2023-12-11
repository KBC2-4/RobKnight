using System;
using Unity.VisualScripting;
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
    private GameObject lastSelectedButton; // 最後に選択されたボタン
    [SerializeField] private GameObject _settingsPanel;
    // [SerializeField] private SettingsAnimatorController _settingsAnimatorController;


    void Start()
    {
        // ゲームの時間を初期状態で通常の速度に設定
        Time.timeScale = 1f;
        // ポーズメニューのUIを非表示に設定
        pauseMenu.SetActive(false);

        // 設定画面を非アクティブに設定
        _settingsPanel.SetActive(false);

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

    // <summary>
    // 「設定」ボタンから呼び出される 設定画面を開く
    // </summary>
    public void ShowSettingsPanel()
    {
        _settingsPanel.SetActive(true);
    }

    // <summary>
    // 「戻る」ボタンから呼び出される 設定画面を閉じる
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
            // ステートを変更
            UIManager.Instance.ChangeState(UIManager.UIState.PauseMenu);

            // ポーズ前の選択を保存
            lastSelectedButton = EventSystem.current.currentSelectedGameObject;

            // ゲームの時間を停止
            Time.timeScale = 0f;
            // ポーズメニューのUIを表示
            pauseMenu.SetActive(true);
            // デフォルトのボタンを選択する
            EventSystem.current.SetSelectedGameObject(defaultButton);
            
            if (_depthOfField != null)
            {
                _depthOfField.active = true;
            }
        }
        else
        {
            // ステートが設定画面の場合、設定画面を閉じる
            if (UIManager.Instance.GetCurrentState() == UIManager.UIState.SettingsMenu)
            {
                _settingsPanel.SetActive(false);
            }

            // 前回のステートに戻す

            // ゲーム画面のステートに戻す
            UIManager.Instance.ChangeState(UIManager.UIState.Gameplay);

            if (lastSelectedButton != null)
            {
                // SEが再生されないようにする
                UISoundManager.Instance.SetProgrammaticSelect();
                // ポーズ前の選択に戻す
                EventSystem.current.SetSelectedGameObject(lastSelectedButton);
            }

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
