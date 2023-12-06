using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneralUIHandler : MonoBehaviour
{
    [Header("シーン管理")]
    [SerializeField] string sceneToLoad;

    [Header("キャンバス管理")]
    [SerializeField] GameObject canvasToShow;
    [SerializeField] GameObject canvasToHide;

    [Header("サウンド管理")]
    [SerializeField] AudioClip soundToPlay;
    private AudioSource audioSource;

    [Header("アニメーション管理")]
    [SerializeField] Animator animator;
    [SerializeField] string animationToPlay;

    [Header("Web管理")]
    [SerializeField] string urlToOpen;

    [Header("トグル管理")]
    [SerializeField] GameObject toggleGameObject;

    private bool isGamePaused = false;

    void Start()
    {
        // AudioSourceコンポーネントを取得または追加
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// シーンのロード
    /// </summary>
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// キャンバスの切り替え
    /// </summary>
    public void SwitchCanvas()
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(true);

        if (canvasToHide != null)
            canvasToHide.SetActive(false);
    }

    /// <summary>
    /// サウンドエフェクトを再生
    /// </summary>
    public void PlaySoundEffect()
    {
        if (soundToPlay != null)
        {
            audioSource.clip = soundToPlay;
            audioSource.Play();
        }
    }

    /// <summary>
    /// アニメーションの再生
    /// </summary>
    public void PlayAnimation()
    {
        if (animator != null && !string.IsNullOrEmpty(animationToPlay))
        {
            animator.Play(animationToPlay);
        }
    }

    /// <summary>
    /// Webページを開く
    /// </summary>
    public void OpenWebPage()
    {
        if (!string.IsNullOrEmpty(urlToOpen))
        {
            Application.OpenURL(urlToOpen);
        }
    }

    /// <summary>
    /// トグル切り替え
    /// </summary>
    public void ToggleGameObject()
    {
        if (toggleGameObject != null)
        {
            toggleGameObject.SetActive(!toggleGameObject.activeSelf);
        }
    }

    /// <summary>
    /// ゲームの一時停止/再開
    /// </summary>
    public void PauseOrResumeGame()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0 : 1;
    }
}
