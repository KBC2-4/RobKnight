using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private GameObject uiForKeyboardMouse; // �L�[�{�[�h���}�E�X�p��UI�I�u�W�F�N�g
    [SerializeField] private GameObject uiForGameController; // �Q�[���R���g���[���[�p��UI�I�u�W�F�N�g
    [SerializeField] private GameObject uiForMobile; // ���o�C���p��UI�I�u�W�F�N�g
    // public GameObject uiPrompt;
    private InputDevice lastActiveDevice; // �O��A�N�e�B�u�ɂȂ����f�o�C�X
    
    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        DeviceTypeDetector.OnDeviceTypeChanged += OnDeviceTypeChanged;
    }

    private void Start()
    {
        UpdateUI(false); // ������Ԃ�UI���\���ɂ���
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
        DeviceTypeDetector.OnDeviceTypeChanged -= OnDeviceTypeChanged;
    }

    private void Update()
    {
        var currentDevice = InputSystem.GetDevice<Keyboard>() ?? (InputDevice)InputSystem.GetDevice<Mouse>();
        if (currentDevice != null && lastActiveDevice != currentDevice)
        {
            lastActiveDevice = currentDevice;
            UpdateUI(true);
        }

        if (Input.anyKey)
        {
            //Debug.Log("Any Key");
        }

        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                Debug.Log("Touch");
            }
        }
        
        // if (Input.touchCount > 0) {
        //     Touch touch = Input.GetTouch(0);
        //
        //     // �^�b�`�̏�Ԃɉ���������
        //     if (touch.phase == TouchPhase.Began) {
        //         Debug.Log("��ʂɃ^�b�`����܂���");
        //     }
        // }

        if (Input.GetAxis("InputGamePadHorizontal") != 0 || Input.GetAxis("InputGamePadVertical") != 0)
        {
            Debug.Log("Axis");
        }
        
        // // �Q�[���p�b�h���ڑ�����Ă��邩�`�F�b�N
        // if (Input.GetJoystickNames().Length > 0) {
        //     // �Q�[���p�b�h�p�̓��͏���
        //     float h = Input.GetAxis("Horizontal");
        //     float v = Input.GetAxis("Vertical");
        //     Debug.Log($"�W���C�X�e�B�b�N: ���� {h}, ���� {v}");
        // }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added || change == InputDeviceChange.Removed || 
            change == InputDeviceChange.Enabled || change == InputDeviceChange.Disabled)
        {
            UpdateUI(uiForKeyboardMouse.activeSelf || uiForGameController.activeSelf || uiForMobile.activeSelf);
        }
    }
    
    private void UpdateUI(bool show)
    {
        // bool isGamepad = Gamepad.current != null;
        // bool isKeyboardMouse = Keyboard.current != null && Mouse.current != null;
        // bool isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        // bool isDesktop = Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application
        
        bool isGamepad = Gamepad.current != null && lastActiveDevice is Gamepad;
        bool isKeyboardMouse = (Keyboard.current != null || Mouse.current != null) && lastActiveDevice is Keyboard || lastActiveDevice is Mouse;
        bool isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

        uiForKeyboardMouse.SetActive(show && isKeyboardMouse && !isMobile);
        uiForGameController.SetActive(show && isGamepad && !isMobile);
        //Debug.Log("���o�C���F " + isMobile);
        //Debug.Log("�L�[�{�[�h���}�E�X�F " + isKeyboardMouse);
        //Debug.Log("�Q�[���R���g���[���[�F " + isGamepad);
        uiForMobile.SetActive(show && isMobile);
    }
    
    void OnTriggerEnter(Collider other)
    {
        // �v���C���[���߂Â�����
        if (other.tag == "Player")
        {
            // UI��\��
            UpdateUI(true); // UI��\��
            // uiPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // �v���C���[�����ꂽ��
        if (other.tag == "Player")
        {
            // UI���\��
            UpdateUI(false); // UI���\��
            // uiPrompt.SetActive(false);
        }
    }
    
    private void OnDeviceTypeChanged(InputDevice device)
    {
        // �f�o�C�X�^�C�v���ύX���ꂽ�Ƃ��̏���
        Debug.Log("Device type changed: " + device);
        
        if (device is Keyboard || device is Mouse)
        {
            ActivateUI(uiForKeyboardMouse);
        }
        else if (device is Gamepad)
        {
            ActivateUI(uiForGameController);
        }
        else if (device is Touchscreen)
        {
            ActivateUI(uiForMobile);
        }
    }
    
    private void ActivateUI(GameObject uiGroup)
    {
        // ���ׂĂ�UI�O���[�v���A�N�e�B�u�ɂ��܂�
        uiForKeyboardMouse.SetActive(false);
        uiForGameController.SetActive(false);
        uiForMobile.SetActive(false);

        // �w�肳�ꂽUI�O���[�v���A�N�e�B�u�ɂ��܂�
        if (uiGroup != null)
        {
            uiGroup.SetActive(true);
        }
    }
}
