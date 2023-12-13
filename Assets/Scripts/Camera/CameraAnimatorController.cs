using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAnimatorController : MonoBehaviour
{
    [SerializeField] private GameObject _cameraParent;  // カメラの親オブジェクト
    [SerializeField, Tooltip("ボスのTransform")] private Transform _bossTransform; // ボスのTransform
    [SerializeField, Tooltip("回転速度")] private float _rotationSpeed = 30f; // 回転速度
    private Vector3 _startPos;
    private Animator _animator;
    private bool _isAnimating = true; // アニメーション中かどうかを追跡するフラグ
    public event Action OnAnimationComplete;   //  アニメーションが終了したときに発行されるイベント

    private void Awake()
    {
        if (_cameraParent != null)
        {

            _startPos = _cameraParent.transform.position;
            _cameraParent.SetActive(false);
            _animator = _cameraParent.GetComponent<Animator>();
        }
    }

    public void StartRotation()
    {
        if (_cameraParent != null)
        {
            _cameraParent.SetActive(true);
        }
        // StartCoroutine(RotateAroundBoss());
    }

    IEnumerator RotateAroundBoss()
    {
        // カメラの初期位置を設定
        //Vector3 bossPosition = _bossTransform.position;
        //Vector3 cameraPosition = bossPosition + new Vector3(0, 5, -10); // ボスの少し上と後ろに配置
        //transform.position = cameraPosition;

        while (true) // 無限ループ
        {
            // ボスの方を向く
            _cameraParent.transform.GetChild(0).transform.LookAt(_bossTransform);
            // ボスを中心に回転させる
            _cameraParent.transform.GetChild(0).transform.RotateAround(_bossTransform.position, Vector3.up, _rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }


public void Animate()
    {
        _cameraParent.transform.position = _startPos;
    }

    public void Animate(Vector3 pos)
    {
        _cameraParent.transform.position = pos;
    }

    public void Animate(Vector3 pos, float time)
    {
        _cameraParent.transform.position = Vector3.Lerp(_startPos, pos, time);
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 bossPosition = _bossTransform.position;
        Vector3 cameraPosition = bossPosition + new Vector3(0, 1.8f, -6.1111f); // ボスの少し上と後ろに配置
        _cameraParent.transform.position = cameraPosition;
    }

    private void Update()
    {
        // アニメーションが終了したかどうかをチェック
        if (_isAnimating && !_animator.IsInTransition(0) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            _isAnimating = false;
            _animator.enabled = false; // Animatorを無効化
            OnAnimationComplete?.Invoke();
            StartCoroutine(RotateAroundBoss());
        }
    }
}
