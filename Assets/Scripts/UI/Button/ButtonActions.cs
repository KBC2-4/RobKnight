using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    /// <summary>
    /// シーンの移動
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    /// <summary>
    /// キャンバスの切り替え
    /// </summary>
    /// <param name="canvas">キャンバス名</param>
    public void ToggleCanvas(GameObject canvas)
    {
        canvas.SetActive(!canvas.activeSelf);
    }

    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="sound">再生するサウンドクリップ</param>
    public void PlaySound(AudioClip sound)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
    }
    
    /// <summary>
    /// ゲームを終了させる
    /// </summary>
    public void QuitGame()
    {
        // ゲームを終了する
        Application.Quit();

        // エディタで実行中の場合のみ実行される
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}