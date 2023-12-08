using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundTrigger : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
{
    [SerializeField] private AudioClip _hoverSound; // �z�o�[����AudioClip
    [SerializeField] private AudioClip _clickSound; // �N���b�N����AudioClip

    public void OnPointerEnter(PointerEventData eventData)
    {
        // �C�x���g���}�E�X�ɂ����̂��ǂ������`�F�b�N
        if (eventData.pointerCurrentRaycast.isValid)
        {
            if (_hoverSound != null)
            {
                // �J�X�^���z�o�[�����Đ�
                UISoundManager.Instance.PlaySound(_hoverSound);
            }
            else
            {
                // �f�t�H���g�̃z�o�[�����Đ�
                UISoundManager.Instance.PlayHoverSound();
            }
            Debug.Log("OnPointerEnter");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_clickSound != null)
        {
            // �J�X�^���N���b�N�����Đ�
            UISoundManager.Instance.PlaySound(_clickSound);
        }
        else
        {
            // �f�t�H���g�̃N���b�N�����Đ�
            UISoundManager.Instance.PlayClickSound();
        }
        Debug.Log("OnPointerClick");
    }

    public void OnSelect(BaseEventData eventData)
    {
        // ���[�U�[�̑���ɂ����̂����`�F�b�N
        // ���݂̓��̓��W���[�����`�F�b�N
        //if (EventSystem.current.currentInputModule is StandaloneInputModule || EventSystem.current.currentInputModule is TouchInputModule)
        //{
        //if (eventData is PointerEventData)
        //{
        if(!UISoundManager.Instance.isProgrammaticSelect)
        {
            if (_hoverSound != null)
            {
                // �J�X�^���z�o�[�����Đ�
                UISoundManager.Instance.PlaySound(_hoverSound);
            }
            else
            {
                // �f�t�H���g�̃z�o�[�����Đ�
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
            // �J�X�^���N���b�N�����Đ�
            UISoundManager.Instance.PlaySound(_clickSound);
        }
        else
        {
            // �f�t�H���g�̃N���b�N�����Đ�
            UISoundManager.Instance.PlayClickSound();
        }
        Debug.Log("OnSubmit");
        //throw new System.NotImplementedException();
    }
}