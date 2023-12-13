using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーのTransform")] private Transform _playerTransform; // プレイヤーのTransform
    [SerializeField, Tooltip("ボスのTransform")] private Transform _bossTransform; // ボスのTransform
    [SerializeField, Tooltip("回転速度")] private float _rotationSpeed = 50f; // 回転速度

    void Awake()
    {
        if (_playerTransform == null)
        {
            _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        }
    }

    public void StartRotation()
    {
        StartCoroutine(RotateAroundBoss());
    }

    IEnumerator RotateAroundBoss()
    {
        // カメラの初期位置を設定
        Vector3 bossPosition = _bossTransform.position;
        Vector3 cameraPosition = bossPosition + new Vector3(0, 5, -10); // ボスの少し上と後ろに配置
        transform.position = cameraPosition;

        while (true) // 無限ループ
        {
            // ボスの方を向く
            transform.LookAt(_bossTransform);
            // ボスを中心に回転させる
            transform.RotateAround(_bossTransform.position, Vector3.up, _rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
