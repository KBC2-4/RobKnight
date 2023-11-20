using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSManager : MonoBehaviour
{
    [SerializeReference] int targetFrameRate = 60;
    
    // Start is called before the first frame update
    void Start()
    {
        SetFrameRate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void SetFrameRate()
    {
        // VSync�i���������j�𖳌���
        QualitySettings.vSyncCount = 0;
        
        // �ڕW�Ƃ���FPS��ύX
        Application.targetFrameRate = targetFrameRate;
        
        // ���݂̃V�[�������擾
        string sceneName = SceneManager.GetActiveScene().name;

        // if (sceneName == "Title") // �^�C�g���V�[��
        // {
        //     Application.targetFrameRate = 30;
        // }
        // else if (sceneName == "GameMainScene") // �Q�[�����C���V�[��
        // {
        //     Application.targetFrameRate = 60;
        // }
    }
}
