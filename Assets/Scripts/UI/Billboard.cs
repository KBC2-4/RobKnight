using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        // メインカメラを取得
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up);
            
            // 常にカメラと同じ向きに設定
            //transform.rotation = mainCamera.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Main Cameraがありません。\nMain Cameraタグを設定してください");
        }
    }
}