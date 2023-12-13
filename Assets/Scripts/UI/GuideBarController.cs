using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

public class GuideBarController : MonoBehaviour
{
    [SerializeField] private GameObject _guideCanvas;   // ガイド用のキャンバス
    [SerializeField] private GameObject keyboardMouseUI; // キーボード＆マウス用のUIオブジェクト
    [SerializeField] private GameObject gamepadUI; // ゲームコントローラー用のUIオブジェクト
    [SerializeField] private GameObject touchUI; // モバイル用のUIオブジェクト
    private Animator _animator;
    private bool _isDisplay = false; // UIを表示するかどうかのフラグ
    [SerializeField] IntroCamera _introCamera; // イントロカメラ

    [SerializeField] private InputActionAsset _inputActions; // Input Action Assetへの参照

    private float _noInputTimer = 0f;
    // private const float noInputThreshold = 1f; // 入力がないと判断する時間
    [SerializeField, Range(0f, 10f)] private float noInputThreshold = 3f; // 入力がないと判断する時間
    [SerializeField,Header("常に表示")] private bool _isAlwayDisplay = false; // 常に表示するかどうかのフラグ

    public static GuideBarController Instance { get; private set; } // シングルトンインスタンス

    public enum GuideName
    {
        Pause, // ポーズ
        Move,   // プレイヤー移動
        Zoom,   // ミニマップ拡大縮小
        Attack, // 攻撃
        Return, // 人間に戻る
        Possession, // 憑依
    }

    // ボタンガイドと対応するGameObjectのマッピング
    private Dictionary<GuideName, GameObject> _buttonGuides = new Dictionary<GuideName, GameObject>();

    // ガイドの表示状態を追跡する辞書
    private Dictionary<GuideName, bool> _guideStates = new Dictionary<GuideName, bool>();


    /// <summary>
    /// オブジェクトが初めてアクティブになった時に一度だけ呼び出される。
    /// シングルトンパターンを実装し、必要に応じてUIを非表示にし、アニメータを設定する。
    /// </summary>
    void Awake()
    {
        // シングルトンの実装
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);

            // シーンがロードされたときに呼ばれるメソッドを登録
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        // Animatorを取得する
        _animator = _guideCanvas.GetComponent<Animator>();
    }

    // シーンがロードされたときに呼ばれるメソッド
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetUIForNewScene(scene.name);
    }

    // 新しいシーン用にUIをリセットするメソッド
    private void ResetUIForNewScene(string sceneName)
    {
        // タイトルシーンの場合は、イントロアニメーションがないのでそのまま表示する
        if (sceneName == "Title")
        {
            _isDisplay = true;
            _guideCanvas.SetActive(true);
            // GuideSet(GuideName.Move, GuideName.Attack, GuideName.Possession);
            // UpdateUI(DeviceTypeDetector.Instance.GetCurrentDevice());
        }
    }

    void Start()
    {

        // UIを非表示にする
        _guideCanvas.SetActive(false);

        // シーン名を取得
        string sceneName = SceneManager.GetActiveScene().name;

        // タイトルシーンの場合は、イントロアニメーションがないのでそのまま表示する
        if (sceneName == "Title")
        {
            _isDisplay = true;
            _guideCanvas.SetActive(true);
            GuideSet(GuideName.Move,GuideName.Attack,GuideName.Possession);
                        // ガイドの表示・非表示状態を適用
            foreach (var guidePair in _buttonGuides)
            {
                if (_guideStates.TryGetValue(guidePair.Key, out bool isActive))
                {
                    guidePair.Value.SetActive(isActive);
                }
            }
            // SetUIVisibility();
        }
    }

    /// <summary>
    /// 指定されたUIパネルがアクティブな場合、そのUIパネルに関連するボタンガイドのマッピングを更新する。
    /// </summary>
    /// <param name="activePanel">アクティブなUIパネル</param>
    private void UpdateGuides(GameObject activePanel)
    {

        //if (_activePanel.activeSelf)
        //{
        //    parentName = "UI/GuideCanvas" + _activePanel.name;
        //}

        // 親オブジェクトも含めてアクティブか確認
        if (activePanel.activeInHierarchy)
        {

            // 各ボタンガイドと対応するGameObjectをマッピング
            //_buttonGuides.Add("Attack", activePanel.transform.Find("AttackAction").gameObject);
            //_buttonGuides.Add("Return", activePanel.transform.Find("ReturnAction").gameObject);
            //_buttonGuides.Add("Possession", activePanel.transform.Find("PossessionAction").gameObject);

            // 各ボタンガイドと対応するGameObjectをマッピング（既存のキーがあれば上書き）
            _buttonGuides[GuideName.Pause] = activePanel.transform.Find("PauseAction").gameObject;
            _buttonGuides[GuideName.Move] = activePanel.transform.Find("MoveAction").gameObject;
            _buttonGuides[GuideName.Zoom] = activePanel.transform.Find("ZoomAction").gameObject;
            _buttonGuides[GuideName.Attack] = activePanel.transform.Find("AttackAction").gameObject;
            _buttonGuides[GuideName.Return] = activePanel.transform.Find("ReturnAction").gameObject;
            _buttonGuides[GuideName.Possession] = activePanel.transform.Find("PossessionAction").gameObject;

            // 初期状態ではすべてのボタンガイドを非表示にする
            //foreach (var guide in _buttonGuides.Values)
            //{
            //    Debug.Log(guide.name);
            //    guide.SetActive(false);
            //}

            // ガイドの表示・非表示状態を適用
            foreach (var guidePair in _buttonGuides)
            {
                if (_guideStates.TryGetValue(guidePair.Key, out bool isActive))
                {
                    guidePair.Value.SetActive(isActive);
                }
            }
        }
    }

    /// <summary>
    /// オブジェクトがアクティブになった時に呼び出される。
    /// アニメーションの設定、入力アクションの有効化、デバイスタイプの変更イベントの登録を行う。
    /// </summary>
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

    /// <summary>
    /// オブジェクトが非アクティブになった時に呼び出される。
    /// アニメーションの設定、入力アクションの無効化、デバイスタイプの変更イベントの解除を行う。
    /// </summary>
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

    /// <summary>
    /// イントロアニメーションが完了した際に呼び出されるハンドラー。この関数ではUIを表示状態に切り替えます。
    /// イントロアニメーションの完了を検知して、UI表示フラグをtrueに設定し、ガイドキャンバスをアクティブにします。
    /// </summary>
    private void HandleIntroAnimationComplete()
    {
        // イントロアニメーション完了後にUIを表示
        _isDisplay = true;
        _guideCanvas.SetActive(true);
        // SetUIVisibility();
    }

    // <summary>
    // 指定されたUIをアクティブにし、他のUIを非アクティブにする
    // </summary>
    // <param name="activePanel">アクティブなUI</param>
    private void SetActivePanel(GameObject activePanel)
    {
        keyboardMouseUI.SetActive(activePanel == keyboardMouseUI);
        gamepadUI.SetActive(activePanel == gamepadUI);
        touchUI.SetActive(activePanel == touchUI);

        // ボタンガイドの初期化
        UpdateGuides(activePanel);
    }

    /// <summary>
    /// 現在の入力デバイスに基づいて適切なUIパネルをアクティブにする。
    /// </summary>
    /// <param name="device">InputDevice</param>
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


    /// <summary>
    /// 毎フレーム呼び出され、入力の有無に基づいてUIの表示を制御する。
    /// </summary>
    private void Update()
    {

        if (IsAnyInput())
        {
            // 何か入力があったとき
            _noInputTimer = 0f;
            _animator.SetBool("IsDisplay", false);
            // _animator.SetBool("IsDisplay", _isDisplay);
            // _guideCanvas.SetActive(false);
            _isDisplay = false;
        }
        else
        {
            // 入力がない場合、タイマーを増加
            _noInputTimer += Time.deltaTime;

            if (_noInputTimer >= noInputThreshold)
            {
                // 指定時間以上入力がない場合、UIを表示
                _animator.SetBool("IsDisplay", true);
                // _guideCanvas.SetActive(true);
                _isDisplay = true;
            }
        }
    }

    /// <summary>
    /// 任意のキーボード、マウス、ゲームパッドの入力があるかどうかを確認する。
    /// 使用例：入力がある場合はUIを非表示にし、ない場合は表示するための判断に使用。
    /// </summary>
    /// <returns>入力がある場合はtrue、それ以外はfalse</returns>
    private bool IsAnyInput()
    {
        // 常に表示フラグが立っている場合は入力を検知しない
        if (_isAlwayDisplay)
        {
            return false;
        }

        // キーボード、マウス、ゲームパッドの入力をチェック
        return Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Gamepad.current?.allControls.Any(control => control.IsPressed()) == true;
    }

    /// <summary>
    /// 指定された名前のガイドをアクティブにする。
    /// 使用例：特定の操作が必要な場面で、関連する操作ガイドを表示するために使用。
    /// </summary>
    /// <param name="guideName">アクティブにするガイドの名前（GuideName enum）</param>
    public void AddGuide(GuideName guideName)
    {
        if (_buttonGuides.TryGetValue(guideName, out GameObject guide))
        {
            guide.SetActive(true);
            _guideStates[guideName] = true;
        }
    }

    /// <summary>
    /// 指定された名前のガイドを非アクティブにする。
    /// 使用例：特定の操作が不要になった場面で、関連する操作ガイドを非表示にするために使用。
    /// </summary>
    /// <param name="guideName">非アクティブにするガイドの名前（GuideName enum）</param>
    public void RemoveGuide(GuideName guideName)
    {
        if (_buttonGuides.TryGetValue(guideName, out GameObject guide))
        {
            guide.SetActive(false);
            _guideStates[guideName] = false;
        }
    }

    /// <summary>
    /// 指定された名前のガイドのみをアクティブにし、他を非アクティブにする。
    /// 使用例：複数の操作が必要なシナリオで、関連する操作ガイドのみを一度に表示するために使用。
    /// </summary>
    /// <param name="guideNames">アクティブにするガイドの名前のセット（GuideName enumの配列）</param>
    public void GuideSet(params GuideName[] guideNames)
    {
        //// すべてのガイドを非表示にする
        //foreach (var guide in _buttonGuides.Values)
        //{
        //    guide.SetActive(false);
        //}

        //// 指定されたガイドのみを表示する
        //foreach (var guideName in guideNames)
        //{
        //    AddGuide(guideName);
        //}

        // すべてのガイドを非表示にし、状態を更新
        foreach (var guidePair in _buttonGuides)
        {
            guidePair.Value.SetActive(false);
            _guideStates[guidePair.Key] = false;
        }

        // 指定されたガイドのみを表示し、状態を更新
        foreach (var guideName in guideNames)
        {
            if (_buttonGuides.TryGetValue(guideName, out GameObject guide))
            {
                guide.SetActive(true);
                _guideStates[guideName] = true;
            }
        }

    }
}
