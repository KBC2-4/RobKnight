using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneralUIHandler : MonoBehaviour
{
    [Header("�V�[���Ǘ�")]
    [SerializeField] string sceneToLoad;

    [Header("�L�����o�X�Ǘ�")]
    [SerializeField] GameObject canvasToShow;
    [SerializeField] GameObject canvasToHide;

    [Header("�T�E���h�Ǘ�")]
    [SerializeField] AudioClip soundToPlay;
    private AudioSource audioSource;

    [Header("�A�j���[�V�����Ǘ�")]
    [SerializeField] Animator animator;
    [SerializeField] string animationToPlay;

    [Header("Web�Ǘ�")]
    [SerializeField] string urlToOpen;

    [Header("�g�O���Ǘ�")]
    [SerializeField] GameObject toggleGameObject;

    private bool isGamePaused = false;

    void Start()
    {
        // AudioSource�R���|�[�l���g���擾�܂��͒ǉ�
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// �V�[���̃��[�h
    /// </summary>
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// �L�����o�X�̐؂�ւ�
    /// </summary>
    public void SwitchCanvas()
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(true);

        if (canvasToHide != null)
            canvasToHide.SetActive(false);
    }

    /// <summary>
    /// �T�E���h�G�t�F�N�g���Đ�
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
    /// �A�j���[�V�����̍Đ�
    /// </summary>
    public void PlayAnimation()
    {
        if (animator != null && !string.IsNullOrEmpty(animationToPlay))
        {
            animator.Play(animationToPlay);
        }
    }

    /// <summary>
    /// Web�y�[�W���J��
    /// </summary>
    public void OpenWebPage()
    {
        if (!string.IsNullOrEmpty(urlToOpen))
        {
            Application.OpenURL(urlToOpen);
        }
    }

    /// <summary>
    /// �g�O���؂�ւ�
    /// </summary>
    public void ToggleGameObject()
    {
        if (toggleGameObject != null)
        {
            toggleGameObject.SetActive(!toggleGameObject.activeSelf);
        }
    }

    /// <summary>
    /// �Q�[���̈ꎞ��~/�ĊJ
    /// </summary>
    public void PauseOrResumeGame()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0 : 1;
    }
}
