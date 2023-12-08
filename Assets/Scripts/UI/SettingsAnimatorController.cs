using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;

public class SettingsAnimatorController : MonoBehaviour
{
    private Animator animator;
    public PostProcessVolume postProcessVolume;
    private UnityEngine.Rendering.PostProcessing.DepthOfField depthOfField;
    //　最初にフォーカスするゲームオブジェクト
    [SerializeField]
    public GameObject firstSelect;
    // ReturnButtonを押した時にフォーカスするゲームオブジェクト
    [SerializeField]
    public GameObject returnSelect;
    private InputControls controls; // インプットコントローラー
    private event Action _onCloseHandler;  // ポーズ画面を閉じたに発生するイベント

    private void Awake()
    {
        controls = new InputControls();

        controls.UI.Cancel.performed += ctx => OnReturnButton();
    }
    
    private void OnEnable()
    {
        controls.UI.Enable();
    }

    private void OnDisable()
    {
        controls.UI.Disable();
        // HideSettings();
    }

    private void OnReturnButton()
    {
        // SEが再生されないようにする
        UISoundManager.Instance.SetProgrammaticSelect();
        // デフォルトのボタンにを選択する
        EventSystem.current.SetSelectedGameObject(firstSelect);
    }
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject!");
        }

        // 初期状態ではぼかしエフェクトをオフにする
        if (postProcessVolume.profile.TryGetSettings(out depthOfField))
        {
            depthOfField.enabled.value = false;
        }
    }

    public void ShowSettings()
    {
        if (animator)
        {
            animator.SetBool("isSettingsOpen", true);
            EnableDepthOfFieldEffect();
            // SEが再生されないようにする
            UISoundManager.Instance.SetProgrammaticSelect();
            EventSystem.current.SetSelectedGameObject(firstSelect);
        }
    }

    public void HideSettings()
    {
        if (animator)
        {
            animator.SetBool("isSettingsOpen", false);
            DisableDepthOfFieldEffect() ;
            // SEが再生されないようにする
            UISoundManager.Instance.SetProgrammaticSelect();
            EventSystem.current.SetSelectedGameObject(returnSelect);
            
        }
    }

    public void EnableDepthOfFieldEffect()
    {
        if (depthOfField != null)
        {
            depthOfField.enabled.value = true;
        }
    }

    public void DisableDepthOfFieldEffect()
    {
        if (depthOfField != null)
        {
            depthOfField.enabled.value = false;
        }
    }
}
