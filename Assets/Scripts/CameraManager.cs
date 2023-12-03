using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject _currentActiveCamera; // 現在アクティブなカメラ
    private GameObject _mainCamera; // メインカメラ

    public static CameraManager Instance;   // インスタンス

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 初期カメラを設定（メインカメラ）
        _currentActiveCamera = GameObject.FindWithTag("MainCamera");
        // メインカメラを格納
        _mainCamera = _currentActiveCamera;
    }

    // カメラを切り替える関数
    public void SwitchCamera(GameObject newCamera)
    {
        if (_currentActiveCamera != null)
        {
            // 現在のカメラを非アクティブにする
            _currentActiveCamera.SetActive(false);
        }

        // 新しいカメラをアクティブにし、現在のカメラとして設定
        newCamera.SetActive(true);
        _currentActiveCamera = newCamera;
    }

    // 現在アクティブなカメラをすべて無効化し、メインカメラに切り替える
    public void SwitchToMainCamera()
    {
        // すべてのカメラを検索
        Camera[] cameras = FindObjectsOfType<Camera>();

        // すべてのカメラを無効化
        foreach (var cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }

        // メインカメラをアクティブ化
        _mainCamera.gameObject.SetActive(true);
    }
}