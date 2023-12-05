using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISoundManager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] AudioSource audioSource; // オーディオソース
    [SerializeField] AudioClip hoverSound;    // カーソル移動音
    [SerializeField] AudioClip clickSound;    // 決定音

    // カーソルが要素に入ったときに呼ばれる
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySound(hoverSound);
    }

    // 要素がクリックされたときに呼ばれる
    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickSound);
    }

    // サウンドを再生するメソッド
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}