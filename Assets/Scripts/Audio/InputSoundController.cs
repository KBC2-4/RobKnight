using UnityEngine;
using UnityEngine.InputSystem;

public class InputSoundController : MonoBehaviour
{
    public InputActionAsset inputActions; // Input Action

    void OnEnable()
    {
        // 各アクションにイベントハンドラーをアサイン
        var actionMap = inputActions.FindActionMap("UI");
        actionMap.FindAction("Submit").performed += _ => UISoundManager.Instance.PlayClickSound();
        actionMap.FindAction("Navigate").performed += _ => UISoundManager.Instance.PlayHoverSound();
    }

    void OnDisable()
    {
        // イベントハンドラーのアンサイン
        var actionMap = inputActions.FindActionMap("UI");
        actionMap.FindAction("Submit").performed -= _ => UISoundManager.Instance.PlayClickSound();
        actionMap.FindAction("Navigate").performed -= _ => UISoundManager.Instance.PlayHoverSound();
    }
}