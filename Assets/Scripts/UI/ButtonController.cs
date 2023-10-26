using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public InputActionAsset inputActions;
    private InputAction submitAction;
    public Button playButton;

    private void Awake()
    {
        // Input Actionsから「Submit」アクションを取得
        submitAction = inputActions.FindActionMap("Menu").FindAction("Submit");

        // 「Submit」アクションが実行されたときに呼ばれるメソッドを設定
        submitAction.performed += _ => OnSubmit();
    }

    private void OnEnable()
    {
        submitAction.Enable();
    }

    private void OnDisable()
    {
        submitAction.Disable();
    }

    private void OnSubmit()
    {
        if (playButton != null && playButton.interactable)
        {
            // ボタンのクリックイベントを実行
            playButton.onClick.Invoke();
        }
    }
}

