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
    
        if (device is Keyboard || device is Mouse)
        {
            Debug.Log("Keyboard & Mouse input detected");
            NotifyDeviceTypeChanged(device);
        }
        else if (device is Gamepad)
        {
            Debug.Log("Gamepad input detected");
            NotifyDeviceTypeChanged(device);
        }
        else if (device is Touchscreen)
        {
            Debug.Log("Touch input detected");
            NotifyDeviceTypeChanged(device);
        }
    }
    
    private static void NotifyDeviceTypeChanged(InputDevice device)
    {
        OnDeviceTypeChanged?.Invoke(device);
    }
}