using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISoundManager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] AudioSource audioSource; // �I�[�f�B�I�\�[�X
    [SerializeField] AudioClip hoverSound;    // �J�[�\���ړ���
    [SerializeField] AudioClip clickSound;    // ���艹

    // �J�[�\�����v�f�ɓ������Ƃ��ɌĂ΂��
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySound(hoverSound);
    }

    // �v�f���N���b�N���ꂽ�Ƃ��ɌĂ΂��
    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickSound);
    }

    // �T�E���h���Đ����郁�\�b�h
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}