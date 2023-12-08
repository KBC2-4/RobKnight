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
    // SEの長さに合わせて調整
    public float delay = 1.0f;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }


    public void OnButtonClick()
    {
        // ボタンの状態が有効の場合
        if (button.interactable) 
        {
            audioSource.PlayOneShot(clickSE);
            // ボタンの状態を無効にする
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
        // ボタンの状態を無効にする
        button.interactable = false;
        SceneManager.LoadScene("GameMainScene");
    }

    public void GoToTitle()
    {
        // ボタンの状態を無効にする
        button.interactable = false;
        SceneManager.LoadScene("Title");
    }
}
