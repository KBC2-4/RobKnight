using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SettingsAnimatorController : MonoBehaviour
{
    private Animator animator;
    public PostProcessVolume postProcessVolume;
    private UnityEngine.Rendering.PostProcessing.DepthOfField depthOfField;

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
        }
    }

    public void HideSettings()
    {
        if (animator)
        {
            animator.SetBool("isSettingsOpen", false);
            DisableDepthOfFieldEffect() ;
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
