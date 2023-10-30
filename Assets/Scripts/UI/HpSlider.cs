using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpSlider : MonoBehaviour
{

    void LateUpdate()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // 常にカメラと同じ向きに設定
            transform.rotation = mainCamera.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Main Cameraがありません。\nMain Cameraタグを設定してください");
        }
    }

}
