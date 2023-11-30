using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class ControlHelpBar : MonoBehaviour
{
    [SerializeField] private GameObject keyboardMouseUI; // キーボード＆マウス用のUIオブジェクト
    [SerializeField] private GameObject gamepadUI; // ゲームコントローラー用のUIオブジェクト
    [SerializeField] private GameObject touchUI; // モバイル用のUIオブジェクト
    // public GameObject uiPrompt;
    private InputDevice lastActiveDevice; // 前回アクティブになったデバイス
    private bool _isDisplay = false; // UIを表示するかどうかのフラグ

    public Transform player; // プレイヤーオブジェクトへの参照
    public float displayDistance = 5.0f; // UIを表示する距離

    void Awake()
    {

        // プレイヤーが見つからない場合、再度検索する
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    private void OnEnable()
    {
        DeviceTypeDetector.OnDeviceTypeChanged += UpdateUI;
    }

    private void OnDisable()
    {
        DeviceTypeDetector.OnDeviceTypeChanged -= UpdateUI;
    }

    private void Update()
    {

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

        // デバイスタイプに基づいてUIを表示・非表示
        if (Keyboard.current != null || Mouse.current != null)
        {
            keyboardMouseUI.SetActive(_isDisplay);
        }
        else if (Gamepad.current != null)
        {
            gamepadUI.SetActive(_isDisplay);
        }
        else if (Touchscreen.current != null)
        {
            touchUI.SetActive(_isDisplay);
        }
    }


    private void UpdateUI(InputDevice device)
    {
        // デバイスタイプに基づいてUIを初期設定
        keyboardMouseUI.SetActive(device is Keyboard || device is Mouse);
        gamepadUI.SetActive(device is Gamepad);
        touchUI.SetActive(device is Touchscreen);
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーが近づいたら
        if (other.tag == "Player")
        {
            // UIを表示
            // _isDisplay = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーが離れたら
        if (other.tag == "Player")
        {
            // UIを非表示
            // _isDisplay = false;
        }
    }
}
