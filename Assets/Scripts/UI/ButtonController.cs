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
        // Input Actions����uSubmit�v�A�N�V�������擾
        submitAction = inputActions.FindActionMap("Menu").FindAction("Submit");

        // �uSubmit�v�A�N�V���������s���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h��ݒ�
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
            // �{�^���̃N���b�N�C�x���g�����s
            playButton.onClick.Invoke();
        }
    }
}

