using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform bossTransform; // ボスのTransform
    [SerializeField] private float rotationSpeed = 1f; // 回転速度

    public void StartRotation()
    {
        StartCoroutine(RotateAroundBoss());
    }

    IEnumerator RotateAroundBoss()
    {
        // カメラの初期位置を設定
        Vector3 bossPosition = bossTransform.position;
        Vector3 cameraPosition = bossPosition + new Vector3(0, 5, -10); // ボスの少し上と後ろに配置
        transform.position = cameraPosition;

        while (true) // 無限ループ
        {
            // ボスの方を向く
            transform.LookAt(bossTransform);
            // ボスを中心に回転させる
            transform.RotateAround(bossTransform.position, Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
