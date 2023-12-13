using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISoundManager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public static UISoundManager Instance { get; private set; }

    [SerializeField] AudioSource audioSource; // オーディオソース
    [SerializeField] AudioClip hoverSound;    // カーソル移動音
    [SerializeField] AudioClip clickSound;    // 決定音

    private bool _isProgrammaticSelect = false;  // SEの再生を無効化するフラグ

    // <sumary>
    // SEの再生を無効化する
    // </summary>
    public void SetProgrammaticSelect()
    {
        _isProgrammaticSelect = true;
    }

    public bool isProgrammaticSelect
    {
        get { return _isProgrammaticSelect; }
        set { _isProgrammaticSelect = value; }
    }

    void Awake()
    {
        // GameObjectをルートに移動
        // transform.parent = null;

        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // シーンでEventSystemで最初に選択しているボタンを設定している場合にSEが再生されないようにtrueにする
        _isProgrammaticSelect = true;
    }

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
    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
            // 400ミリ秒（0.4秒）で振動
            //VibrationController.Instance.VibrateForDuration(400, 1.0f);
        }
    }

    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
        // 200ミリ秒（0.2秒）で振動
        //VibrationController.Instance.VibrateForDuration(200, 0.1f);
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
        // 400ミリ秒（0.4秒）で振動
        //VibrationController.Instance.VibrateForDuration(400, 1.0f);
    }
}