using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

public class ControlHelpBar : MonoBehaviour
{
    [SerializeField] private GameObject _guideCanvas;   // ガイド用のキャンバス
    [SerializeField] private GameObject keyboardMouseUI; // キーボード＆マウス用のUIオブジェクト
    [SerializeField] private GameObject gamepadUI; // ゲームコントローラー用のUIオブジェクト
    [SerializeField] private GameObject touchUI; // モバイル用のUIオブジェクト
    private Animator _animator;
    // public GameObject uiPrompt;
    private InputDevice lastActiveDevice; // 前回アクティブになったデバイス
    private bool _isDisplay = false; // UIを表示するかどうかのフラグ
    [SerializeField] IntroCamera _introCamera; // イントロカメラ

    [SerializeField] private InputActionAsset _inputActions; // Input Action Assetへの参照

    private float _noInputTimer = 0f;
    private const float noInputThreshold = 1f; // 入力がないと判断する時間

    public static ControlHelpBar Instance { get; private set; }

    private Dictionary<string, GameObject> _buttonGuides = new Dictionary<string, GameObject>();


    void Awake()
    {
        // シングルトンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // ボタンガイドの初期化
        InitializeGuides();


        // UIを非表示にする
        _guideCanvas.SetActive(false);

        // Animatorを取得する
        _animator = _guideCanvas.GetComponent<Animator>();

        // シーン名を取得
        string sceneName = SceneManager.GetActiveScene().name;

        // タイトルシーンの場合は、イントロアニメーションがないのでそのまま表示する
        if (sceneName == "Title")
        {
            _isDisplay = true;
            _guideCanvas.SetActive(true);
            // gamepadUI.SetActive(true);
            // SetUIVisibility();
        }
    }

    private void InitializeGuides()
    {
        // 各ボタンガイドと対応するGameObjectをマッピング
        //_buttonGuides.Add("Attack", attackButtonUI);
        //_buttonGuides.Add("Jump", jumpButtonUI);
        //_buttonGuides.Add("Guard", guardButtonUI);

        // 初期状態ではすべてのボタンガイドを非表示にする
        foreach (var guide in _buttonGuides.Values)
        {
            guide.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _animator.SetBool("IsDisplay", _isDisplay);

        _inputActions.Enable();

        DeviceTypeDetector.OnDeviceTypeChanged += UpdateUI;

        if (_introCamera != null)
        {
            _introCamera.OnIntroAnimationComplete += HandleIntroAnimationComplete;
        }
    }

    private void OnDisable()
    {
        _animator.SetBool("IsDisplay", _isDisplay);

        _inputActions.Disable();

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
        _guideCanvas.SetActive(true);
        // SetUIVisibility();
    }

    private void UpdateUIBasedOnInputDevice()
    {
        if (Gamepad.current != null)
        {
            SetActivePanel(gamepadUI);
        }
        else if (Keyboard.current != null && Mouse.current != null)
        {
            SetActivePanel(keyboardMouseUI);
        }
        else if (Touchscreen.current != null)
        {
            SetActivePanel(touchUI);
        }
    }

    private void SetActivePanel(GameObject activePanel)
    {
        keyboardMouseUI.SetActive(activePanel == keyboardMouseUI);
        gamepadUI.SetActive(activePanel == gamepadUI);
        touchUI.SetActive(activePanel == touchUI);
    }


    private void UpdateUI(InputDevice device)
    {

        if (device is Keyboard || device is Mouse)
        {
            // キーボードとマウス用のUIに切り替え
            // keyboardMouseUI.SetActive(_isDisplay);
            SetActivePanel(keyboardMouseUI);
        }
        else if (device is Gamepad)
        {
            // ゲームパッド用のUIに切り替え
            // gamepadUI.SetActive(_isDisplay);
            SetActivePanel(gamepadUI);
        }
        else if (device is Touchscreen)
        {
            // タッチスクリーン用のUIに切り替え
            // touchUI.SetActive(_isDisplay);
            SetActivePanel(touchUI);
        }
    }


    private void Update()
    {
        // UpdateUIBasedOnInputDevice();

        if (IsAnyInput())
        {
            // 何か入力があったとき
            _noInputTimer = 0f;
            _guideCanvas.SetActive(false);
            _isDisplay = false;
        }
        else
        {
            // 入力がない場合、タイマーを増加
            _noInputTimer += Time.deltaTime;

            if (_noInputTimer >= noInputThreshold)
            {
                // 指定時間以上入力がない場合、UIを表示
                _guideCanvas.SetActive(true);
                _isDisplay = true;
            }
        }
    }

    private bool IsAnyInput()
    {
        // キーボード、マウス、ゲームパッドの入力をチェック
        return Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Gamepad.current?.allControls.Any(control => control.IsPressed()) == true;
    }

    public void AddGuide(string guideName)
    {
        if (_buttonGuides.TryGetValue(guideName, out GameObject guide))
        {
            guide.SetActive(true);
        }
    }

    public void RemoveGuide(string guideName)
    {
        if (_buttonGuides.TryGetValue(guideName, out GameObject guide))
        {
            guide.SetActive(false);
        }
    }

    public void GuideSet(params string[] guideNames)
    {
        // すべてのガイドを非表示にする
        foreach (var guide in _buttonGuides.Values)
        {
            guide.SetActive(false);
        }

        // 指定されたガイドのみを表示する
        foreach (var guideName in guideNames)
        {
            AddGuide(guideName);
        }
    }
}
