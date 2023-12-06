using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    /// <summary>
    /// �V�[���̈ړ�
    /// </summary>
    /// <param name="sceneName">�V�[����</param>
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    /// <summary>
    /// �L�����o�X�̐؂�ւ�
    /// </summary>
    /// <param name="canvas">�L�����o�X��</param>
    public void ToggleCanvas(GameObject canvas)
    {
        canvas.SetActive(!canvas.activeSelf);
    }

    /// <summary>
    /// SE�̍Đ�
    /// </summary>
    /// <param name="sound">�Đ�����T�E���h�N���b�v</param>
    public void PlaySound(AudioClip sound)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
    }
    
    /// <summary>
    /// �Q�[�����I��������
    /// </summary>
    public void QuitGame()
    {
        // �Q�[�����I������
        Application.Quit();

        // �G�f�B�^�Ŏ��s���̏ꍇ�̂ݎ��s�����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}