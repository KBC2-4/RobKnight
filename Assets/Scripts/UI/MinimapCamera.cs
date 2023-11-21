using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �~�j�}�b�v�쐬�p�J����
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
    /// �J�������擾����
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
        
        // ���s���Ƀ~�j�}�b�v�p�̃J������T��
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
        // �E�X�e�B�b�N�ŃY�[������
        var zoomInput = Gamepad.current.rightStick.y.ReadValue();
        if (!isFixedZoom)
        {
            _camera.orthographicSize -= zoomInput * zoomSpeed * Time.deltaTime;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);
        }
        
        // ���X�e�B�b�N�������݂ŌŒ�Y�[�����x���ɐ؂�ւ�
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