using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    private UIState currentState;  // ���݂̃X�e�[�g
    private UIState previousState; // �O��̃X�e�[�g

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
        Gameplay,       // �Q�[���v���C���
        PauseMenu,      // �|�[�Y���
        SettingsMenu,    // �ݒ���
        GameClear,      // �Q�[���N���A���
        GameOver,        // �Q�[���I�[�o�[���
        Credit,         // �N���W�b�g���
    }

    public void ChangeState(UIState newState)
    {
        if (currentState != newState)
        {
            // ���݂̃X�e�[�g��O��̃X�e�[�g�Ƃ��ĕۑ�
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
        // �O��̃X�e�[�g�ɖ߂�
        ChangeState(previousState);
    }
}
