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
        // VSync（垂直同期）を無効化
        QualitySettings.vSyncCount = 0;
        
        // 目標とするFPSを変更
        Application.targetFrameRate = targetFrameRate;
        
        // 現在のシーン名を取得
        string sceneName = SceneManager.GetActiveScene().name;

        // if (sceneName == "Title") // タイトルシーン
        // {
        //     Application.targetFrameRate = 30;
        // }
        // else if (sceneName == "GameMainScene") // ゲームメインシーン
        // {
        //     Application.targetFrameRate = 60;
        // }
    }
}
