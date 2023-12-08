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
    //�@�ŏ��Ƀt�H�[�J�X����Q�[���I�u�W�F�N�g
    [SerializeField]
    public GameObject firstSelect;
    // ReturnButton�����������Ƀt�H�[�J�X����Q�[���I�u�W�F�N�g
    [SerializeField]
    public GameObject returnSelect;
    private InputControls controls; // �C���v�b�g�R���g���[���[
    private event Action _onCloseHandler;  // �|�[�Y��ʂ�����ɔ�������C�x���g

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
        // SE���Đ�����Ȃ��悤�ɂ���
        UISoundManager.Instance.SetProgrammaticSelect();
        // �f�t�H���g�̃{�^���ɂ�I������
        EventSystem.current.SetSelectedGameObject(firstSelect);
    }
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject!");
        }

        // ������Ԃł͂ڂ����G�t�F�N�g���I�t�ɂ���
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
            // SE���Đ�����Ȃ��悤�ɂ���
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
            // SE���Đ�����Ȃ��悤�ɂ���
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
