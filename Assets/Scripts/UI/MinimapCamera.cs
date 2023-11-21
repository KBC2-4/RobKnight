using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ミニマップ作成用カメラ
/// </summary>
[ExecuteInEditMode]
public class MinimapCamera : MonoBehaviour
{
    private Camera _camera;
    public float zoomSpeed = 10f;
    public float minZoom = 100f;
    public float maxZoom = 1000f;
    public InputActionAsset inputActions;
    private InputAction _lookAction;
    private Vector2 _input;
    public float fixedZoomLevel = 100f;
    private bool isFixedZoom = false;

    /// <summary>
    /// カメラを取得する
    /// </summary>
    public Camera camera
    {
        get
        {
            if (!_camera)
            {
                _camera = GetComponent<Camera>();
            }
            return _camera;
        }
    }

    private void Awake()
    {
        camera.depthTextureMode = DepthTextureMode.Depth;
        
        // 実行時にミニマップ用のカメラを探す
        if (_camera == null)
        {
            _camera = GameObject.FindWithTag("MiniMapCamera").GetComponent<Camera>();
        }
    }

    private void OnEnable()
    {
        _lookAction = inputActions.FindActionMap("Player").FindAction("Look");
        _lookAction.Enable();
    }

    private void OnDisable()
    {
        _lookAction.Disable();
    }

    private void Update()
    {
        // 右スティックでズーム調整
        var zoomInput = Gamepad.current.rightStick.y.ReadValue();
        if (!isFixedZoom)
        {
            _camera.orthographicSize -= zoomInput * zoomSpeed * Time.deltaTime;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);
        }
        
        // 左スティック押し込みで固定ズームレベルに切り替え
        if (Gamepad.current.leftStickButton.wasPressedThisFrame)
        {
            if (isFixedZoom)
            {
                isFixedZoom = false;
            }
            else
            {
                _camera.orthographicSize = fixedZoomLevel;
                isFixedZoom = true;
            }
        }
        
        // _input = _lookAction.ReadValue<Vector2>();
        //
        // if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     _myCamera.orthographicSize -= zoomSpeed * Time.deltaTime;
        // }
        // else if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     _myCamera.orthographicSize += zoomSpeed * Time.deltaTime;
        // }
        //
        // _myCamera.orthographicSize = Mathf.Clamp(_myCamera.orthographicSize, minZoom, maxZoom);
    }
}