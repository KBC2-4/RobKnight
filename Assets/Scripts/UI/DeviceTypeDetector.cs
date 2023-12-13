using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceTypeDetector : MonoBehaviour
{
    // シングルトンインスタンス
    public static DeviceTypeDetector Instance { get; private set; }
    
    // デバイスタイプが変更されたときにトリガーされるイベント
    public delegate void DeviceTypeChanged(InputDevice device);
    public static event DeviceTypeChanged OnDeviceTypeChanged;
    
    public InputAction keyboardMouseAction; // キーボードとマウスの入力を検出するアクション
    public InputAction gamepadAction; // ゲームパッドの入力を検出するアクション
    public InputAction touchAction; // タッチパッドの入力を検出するアクション

    private InputDevice _currentDevice; // 現在のアクティブなデバイス

    private void Awake()
    {
        // シングルトンの初期化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // ゲーム開始時にデバイスタイプを判断
        // コルーチンを開始
        StartCoroutine(DetectInitialDeviceType());
    }

    // ゲーム開始時に利用可能なデバイスを検出するメソッド
    // 初期デバイスタイプを検出するコルーチン
    private IEnumerator DetectInitialDeviceType()
    {
        // Input Systemがデバイスを初期化するまで待機
        yield return new WaitUntil(() => InputSystem.devices.Count > 0);

        var devices = InputSystem.devices;
        InputDevice initialDevice = null;

        // 最初に検出されたデバイスを検出
        //if (devices.Any(device => device is Keyboard || device is Mouse))
        //{
        //    initialDevice = devices.First(device => device is Keyboard || device is Mouse);
        //}
        //else if (devices.Any(device => device is Gamepad))
        //{
        //    initialDevice = devices.First(device => device is Gamepad);
        //}

        // ゲームパッドを優先的に検出
        // ゲームパッドを最初にチェック
        if (devices.Any(device => device is Gamepad))
        {
            initialDevice = devices.First(device => device is Gamepad);
        }
        // キーボード・マウスを次にチェック
        else if (devices.Any(device => device is Keyboard || device is Mouse))
        {
            initialDevice = devices.First(device => device is Keyboard || device is Mouse);
        }

        // 初期デバイスが検出された場合、イベントをトリガー
        if (initialDevice != null)
        {
            // 現在のデバイスを更新
            _currentDevice = initialDevice;
            NotifyDeviceTypeChanged(initialDevice);
        }
    }

    private void OnEnable()
    {
        // アクションの実行時にデバイスの種類を判別するイベントをそれぞれ登録
        keyboardMouseAction.performed += ctx => DetectDeviceType(ctx);
        gamepadAction.performed += ctx => DetectDeviceType(ctx);
        touchAction.performed += ctx => DetectDeviceType(ctx);

        keyboardMouseAction.Enable();
        gamepadAction.Enable();
        touchAction.Enable();
    }

    private void OnDisable()
    {
        keyboardMouseAction.Disable();
        gamepadAction.Disable();
        touchAction.Disable();
    }
    
    private void DetectDeviceType(InputAction.CallbackContext context)
    {
        // 入力デバイスを取得
        var device = context.control.device;

        // 現在のデバイスを更新
        _currentDevice = device;

        if (device is Keyboard || device is Mouse)
        {
            // Debug.Log("Keyboard & Mouse input detected");
            NotifyDeviceTypeChanged(device);
        }
        else if (device is Gamepad)
        {
            // Debug.Log("Gamepad input detected");
            NotifyDeviceTypeChanged(device);
        }
        else if (device is Touchscreen)
        {
            // Debug.Log("Touch input detected");
            NotifyDeviceTypeChanged(device);
        }
    }

    /// <summary>
    /// 現在のデバイスを返すメソッド
    /// </summary>
    /// <returns></returns>
    public InputDevice GetCurrentDevice()
    {
        return _currentDevice;
    }

    private static void NotifyDeviceTypeChanged(InputDevice device)
    {
        OnDeviceTypeChanged?.Invoke(device);
    }
}