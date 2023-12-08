using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangger : MonoBehaviour
{
    //public void BtnOnClick()
    //{
    //    Debug.Log("OK!");
    //    SceneManager.LoadScene("GameMainScene");
    //}

    public AudioSource audioSource;
    public AudioClip clickSE;
    // SE�̒����ɍ��킹�Ē���
    public float delay = 1.0f;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }


    public void OnButtonClick()
    {
        // �{�^���̏�Ԃ��L���̏ꍇ
        if (button.interactable) 
        {
            audioSource.PlayOneShot(clickSE);
            // �{�^���̏�Ԃ𖳌��ɂ���
            button.interactable = false;
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameMainScene");
    }

    public void GoToGameMainScene()
    {
        // �{�^���̏�Ԃ𖳌��ɂ���
        button.interactable = false;
        SceneManager.LoadScene("GameMainScene");
    }

    public void GoToTitle()
    {
        // �{�^���̏�Ԃ𖳌��ɂ���
        button.interactable = false;
        SceneManager.LoadScene("Title");
    }
}
