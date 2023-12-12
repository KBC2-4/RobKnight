using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    private UIState currentState;  // 現在のステート
    private UIState previousState; // 前回のステート

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum UIState
    {
        Gameplay,       // ゲームプレイ画面
        PauseMenu,      // ポーズ画面
        SettingsMenu,    // 設定画面
        GameClear,      // ゲームクリア画面
        GameOver,        // ゲームオーバー画面
        Credit,         // クレジット画面
    }

    public void ChangeState(UIState newState)
    {
        if (currentState != newState)
        {
            // 現在のステートを前回のステートとして保存
            previousState = currentState;
            currentState = newState;
            // UpdateUIState();
        }
    }

    private void UpdateUIState()
    {
        switch (currentState)
        {
            case UIState.Gameplay:
                pauseMenu.SetActive(false);
                settingsMenu.SetActive(false);
                break;
            case UIState.PauseMenu:
                pauseMenu.SetActive(true);
                break;
            case UIState.SettingsMenu:
                settingsMenu.SetActive(true);
                break;
        }
    }

    public UIState GetCurrentState()
    {
        return currentState;
    }

    public void ReturnToPreviousState()
    {
        // 前回のステートに戻す
        ChangeState(previousState);
    }
}
