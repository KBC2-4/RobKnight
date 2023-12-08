using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class ControlHelpBar : MonoBehaviour
{
    [SerializeField] private GameObject keyboardMouseUI; // キーボード＆マウス用のUIオブジェクト
    [SerializeField] private GameObject gamepadUI; // ゲームコントローラー用のUIオブジェクト
    [SerializeField] private GameObject touchUI; // モバイル用のUIオブジェクト
    private Animator _keyboardMouseUIAnimator;
    private Animator _gamepadUIAnimator;
    private Animator _touchUIAnimator;
    // public GameObject uiPrompt;
    private InputDevice lastActiveDevice; // 前回アクティブになったデバイス
    private bool _isDisplay = false; // UIを表示するかどうかのフラグ

    public Transform player; // プレイヤーオブジェクトへの参照
    public float displayDistance = 5.0f; // UIを表示する距離
    [SerializeField] IntroCamera _introCamera; // イントロカメラ

    void Awake()
    {

        // プレイヤーが見つからない場合、再度検索する
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        // 各UIパネルのAnimatorを取得
        _keyboardMouseUIAnimator = keyboardMouseUI.GetComponent<Animator>();
        _gamepadUIAnimator = gamepadUI.GetComponent<Animator>();
        _touchUIAnimator = touchUI.GetComponent<Animator>();


        // 一時
        keyboardMouseUI.SetActive(false);
        gamepadUI.SetActive(false);
        touchUI.SetActive(false);
    }

    private void OnEnable()
    {
        DeviceTypeDetector.OnDeviceTypeChanged += UpdateUI;

        if (_introCamera != null)
        {
            _introCamera.OnIntroAnimationComplete += HandleIntroAnimationComplete;
        }
    }

    private void OnDisable()
    {
        DeviceTypeDetector.OnDeviceTypeChanged -= UpdateUI;

        if (_introCamera != null)
        {
            _introCamera.OnIntroAnimationComplete -= HandleIntroAnimationComplete;
        }
    }

    private void HandleIntroAnimationComplete()
    {
        // イントロアニメーション完了後にUIを表示
        _isDisplay = true;
        gameObject.SetActive(true);
        gamepadUI.SetActive(true);
        SetUIVisibility();
    }

    private void Update()
    {

        //// プレイヤーとUIオブジェクトの距離を計算
        //float distance = Vector3.Distance(player.transform.position, transform.position);

        //// 距離に基づいてUIを表示または非表示
        //if (distance <= displayDistance)
        //{
        //    _isDisplay = true;
        //}
        //else
        //{
        //    _isDisplay = false;
        //}

        // デバイスタイプに基づいてUIを表示・非表示
        //if (Keyboard.current != null || Mouse.current != null)
        //{
        //    keyboardMouseUI.SetActive(_isDisplay);
        //}
        //else if (Gamepad.current != null)
        //{
        //    gamepadUI.SetActive(_isDisplay);
        //}
        //else if (Touchscreen.current != null)
        //{
        //    touchUI.SetActive(_isDisplay);
        //}

        // UIのAnimatorに表示状態を設定
        SetUIVisibility();
    }

    private void SetUIVisibility()
    {
        _keyboardMouseUIAnimator.SetBool("IsDisplay", _isDisplay && Keyboard.current != null);
        _gamepadUIAnimator.SetBool("IsDisplay", _isDisplay && Gamepad.current != null);
        _touchUIAnimator.SetBool("IsDisplay", _isDisplay && Touchscreen.current != null);
    }


    private void UpdateUI(InputDevice device)
    {
        // デバイスタイプに基づいてUIを初期設定
        //keyboardMouseUI.SetActive(device is Keyboard || device is Mouse);
        //gamepadUI.SetActive(device is Gamepad);
        //touchUI.SetActive(device is Touchscreen);

        bool isKeyboardMouse = device is Keyboard || device is Mouse;
        bool isGamepad = device is Gamepad;
        bool isTouchscreen = device is Touchscreen;

        if (_keyboardMouseUIAnimator != null)
        {
            _keyboardMouseUIAnimator.SetBool("IsDisplay", isKeyboardMouse && _isDisplay);
        }

        if (_gamepadUIAnimator != null)
        {
            _gamepadUIAnimator.SetBool("IsDisplay", isGamepad && _isDisplay);
        }

        if (_touchUIAnimator != null)
        {
            _touchUIAnimator.SetBool("IsDisplay", isTouchscreen && _isDisplay);
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    // プレイヤーが近づいたら
    //    if (other.tag == "Player")
    //    {
    //        // UIを表示
    //        // _isDisplay = true;
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    // プレイヤーが離れたら
    //    if (other.tag == "Player")
    //    {
    //        // UIを非表示
    //        // _isDisplay = false;
    //    }
    //}
}
