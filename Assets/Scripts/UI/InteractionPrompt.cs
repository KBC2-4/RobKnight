using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private GameObject uiForKeyboardMouse; // キーボード＆マウス用のUIオブジェクト
    [SerializeField] private GameObject uiForGameController; // ゲームコントローラー用のUIオブジェクト
    [SerializeField] private GameObject uiForMobile; // モバイル用のUIオブジェクト
    // public GameObject uiPrompt;
    private InputDevice lastActiveDevice; // 前回アクティブになったデバイス
    private bool _isDisplay = false; // UIを表示するかどうかのフラグ
    
    public GameObject player; // プレイヤーオブジェクトへの参照
    public float displayDistance = 5.0f; // UIを表示する距離
    
    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        DeviceTypeDetector.OnDeviceTypeChanged += OnDeviceTypeChanged;
    }

    private void Start()
    {
        UpdateUI(false); // 初期状態でUIを非表示にする
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
        //     // タッチの状態に応じた処理
        //     if (touch.phase == TouchPhase.Began) {
        //         Debug.Log("画面にタッチされました");
        //     }
        // }

        if (Input.GetAxis("InputGamePadHorizontal") != 0 || Input.GetAxis("InputGamePadVertical") != 0)
        {
            Debug.Log("Axis");
        }
        
        // // ゲームパッドが接続されているかチェック
        // if (Input.GetJoystickNames().Length > 0) {
        //     // ゲームパッド用の入力処理
        //     float h = Input.GetAxis("Horizontal");
        //     float v = Input.GetAxis("Vertical");
        //     Debug.Log($"ジョイスティック: 水平 {h}, 垂直 {v}");
        // }
        
        // プレイヤーとUIオブジェクトの距離を計算
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // 距離に基づいてUIを表示または非表示
        if (distance <= displayDistance)
        {
            _isDisplay = true;
        }
        else
        {
            _isDisplay = false;
        }
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
        // // bool isGamepad = Gamepad.current != null;
        // // bool isKeyboardMouse = Keyboard.current != null && Mouse.current != null;
        // // bool isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        // // bool isDesktop = Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application
        //
        // bool isGamepad = Gamepad.current != null && lastActiveDevice is Gamepad;
        // bool isKeyboardMouse = (Keyboard.current != null || Mouse.current != null) && lastActiveDevice is Keyboard || lastActiveDevice is Mouse;
        // bool isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        //
        // uiForKeyboardMouse.SetActive(show && isKeyboardMouse && !isMobile);
        // uiForGameController.SetActive(show && isGamepad && !isMobile);
        // //Debug.Log("モバイル： " + isMobile);
        // //Debug.Log("キーボード＆マウス： " + isKeyboardMouse);
        // //Debug.Log("ゲームコントローラー： " + isGamepad);
        // uiForMobile.SetActive(show && isMobile);
    }
    
    void OnTriggerEnter(Collider other)
    {
        // プレイヤーが近づいたら
        if (other.tag == "Player")
        {
            // UIを表示
            // UpdateUI(true); // UIを表示
            // uiPrompt.SetActive(true);
            
            // _isDisplay = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーが離れたら
        if (other.tag == "Player")
        {
            // UIを非表示
            // UpdateUI(false); // UIを非表示
            // uiPrompt.SetActive(false);
            
            // _isDisplay = false;
        }
    }
    
    private void OnDeviceTypeChanged(InputDevice device)
    {
        // デバイスタイプが変更されたときの処理
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
        if (_isDisplay == false){return;}
        
        // すべてのUIグループを非アクティブにします
        uiForKeyboardMouse.SetActive(false);
        uiForGameController.SetActive(false);
        uiForMobile.SetActive(false);

        // 指定されたUIグループをアクティブにします
        if (uiGroup != null)
        {
            uiGroup.SetActive(true);
        }
    }
}
