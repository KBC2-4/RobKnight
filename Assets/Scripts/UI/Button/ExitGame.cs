using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
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
