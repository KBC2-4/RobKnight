using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundTrigger : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
{
    [SerializeField] private AudioClip _hoverSound; // ホバー音のAudioClip
    [SerializeField] private AudioClip _clickSound; // クリック音のAudioClip

    public void OnPointerEnter(PointerEventData eventData)
    {
        // イベントがマウスによるものかどうかをチェック
        if (eventData.pointerCurrentRaycast.isValid)
        {
            if (_hoverSound != null)
            {
                // カスタムホバー音を再生
                UISoundManager.Instance.PlaySound(_hoverSound);
            }
            else
            {
                // デフォルトのホバー音を再生
                UISoundManager.Instance.PlayHoverSound();
            }
            Debug.Log("OnPointerEnter");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_clickSound != null)
        {
            // カスタムクリック音を再生
            UISoundManager.Instance.PlaySound(_clickSound);
        }
        else
        {
            // デフォルトのクリック音を再生
            UISoundManager.Instance.PlayClickSound();
        }
        Debug.Log("OnPointerClick");
    }

    public void OnSelect(BaseEventData eventData)
    {
        // ユーザーの操作によるものかをチェック
        // 現在の入力モジュールをチェック
        //if (EventSystem.current.currentInputModule is StandaloneInputModule || EventSystem.current.currentInputModule is TouchInputModule)
        //{
        //if (eventData is PointerEventData)
        //{
        if(!UISoundManager.Instance.isProgrammaticSelect)
        {
            if (_hoverSound != null)
            {
                // カスタムホバー音を再生
                UISoundManager.Instance.PlaySound(_hoverSound);
            }
            else
            {
                // デフォルトのホバー音を再生
                UISoundManager.Instance.PlayHoverSound();
            }
            Debug.Log("OnSelect");
        }
        else
        {
            UISoundManager.Instance.isProgrammaticSelect = false;
        }

        // }
        // throw new System.NotImplementedException();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (_clickSound != null)
        {
            // カスタムクリック音を再生
            UISoundManager.Instance.PlaySound(_clickSound);
        }
        else
        {
            // デフォルトのクリック音を再生
            UISoundManager.Instance.PlayClickSound();
        }
        Debug.Log("OnSubmit");
        //throw new System.NotImplementedException();
    }
}