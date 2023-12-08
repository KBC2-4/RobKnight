using UnityEngine;
using UnityEngine.InputSystem;

public class InputSoundController : MonoBehaviour
{
    public InputActionAsset inputActions; // Input Action

    void OnEnable()
    {
        // �e�A�N�V�����ɃC�x���g�n���h���[���A�T�C��
        var actionMap = inputActions.FindActionMap("UI");
        actionMap.FindAction("Submit").performed += _ => UISoundManager.Instance.PlayClickSound();
        actionMap.FindAction("Navigate").performed += _ => UISoundManager.Instance.PlayHoverSound();
    }

    void OnDisable()
    {
        // �C�x���g�n���h���[�̃A���T�C��
        var actionMap = inputActions.FindActionMap("UI");
        actionMap.FindAction("Submit").performed -= _ => UISoundManager.Instance.PlayClickSound();
        actionMap.FindAction("Navigate").performed -= _ => UISoundManager.Instance.PlayHoverSound();
    }
}