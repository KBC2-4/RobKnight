using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CredittController : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 20f;
    [SerializeField] private float _fastForwardSpeed = 100f; // 早送りのスピード
    [SerializeField] private TMP_Text _creditsText;
    [SerializeField] private GameObject _creditsPanel;
    private RectTransform _creditsPanelRect;
    private Animator _creditsPanelAnimator;
    private InputAction _aButtonAction; // Aボタン用のInput Action
    private InputControls _inputControls; // Input Actionsクラスのインスタンス

    void Awake()
    {
        _inputControls = new InputControls();
    }

    void OnEnable()
    {
        // Input Actionsを有効化
        _inputControls.Enable();
    }

    void OnDisable()
    {
        // Input Actionsを無効化
        _inputControls.Disable();
    }

    void Start()
    {
        _creditsPanel.SetActive(true);
        _creditsPanelAnimator = _creditsPanel.GetComponent<Animator>();

        // Input Actionの初期化
        _aButtonAction = new InputAction(binding: "<Gamepad>/buttonSouth");
        _aButtonAction.Enable();

        // _creditsPanelからRectTransformコンポーネントを取得
        _creditsPanelRect = _creditsPanel.GetComponent<RectTransform>();

        if (_creditsPanelRect == null)
        {
            // エラーハンドリング
            Debug.LogError("_creditsPanelにRectTransformコンポーネントが見つかりませんでした。");
        }
    }

    void Update()
    {
        // テキストを上にスクロール
        // creditsText.transform.position = new Vector2(_creditsText.transform.position.x, _creditsText.transform.position.y + _scrollSpeed * Time.deltaTime);

        // Aボタンが押されているかチェック
        //bool isAFastForwarding = _aButtonAction.ReadValue<float>() > 0;

        //// 早送りがアクティブの場合は速度を増加させる
        //float currentSpeed = isAFastForwarding ? _fastForwardSpeed : _scrollSpeed;

        // Submitアクションの状態を確認
        bool isSubmitting = _inputControls.UI.Submit.ReadValue<float>() > 0;

        // 早送りがアクティブの場合は速度を増加させる
        float currentSpeed = isSubmitting ? _fastForwardSpeed : _scrollSpeed;

        // テキストを上にスクロール
        _creditsText.transform.position = new Vector2(_creditsText.transform.position.x, _creditsText.transform.position.y + currentSpeed * Time.deltaTime);

        // アニメーションのトリガー（特定の位置でフェードインを開始）
        if (_creditsText.transform.position.y >= 1000)
        {
            // _creditsPanelAnimator.SetTrigger("FadeIn");
        }

        // 文字が画面外に出たらTitleシーンに遷移
        //if (_creditsText.transform.position.y >= 1000) // この値は適宜調整
        //{
        //    SceneManager.LoadScene("Title"); // タイトルシーンに遷移
        //}
        // TextがPanelの範囲外に出たか確認
        if (!IsTextWithinPanel(_creditsText.rectTransform, _creditsPanelRect))
        {
            Debug.Log("範囲外に出ました");
            SceneManager.LoadScene("Title"); // タイトルシーンに遷移
        }
    }

    // TextがPanelの範囲内にあるかどうかを判断するメソッド
    private bool IsTextWithinPanel(RectTransform textRect, RectTransform panelRect)
    {
        // Panelのワールド空間でのコーナーを取得
        Vector3[] panelCorners = new Vector3[4];
        panelRect.GetWorldCorners(panelCorners);

        // Panelの範囲を定義
        float panelLeft = panelCorners[0].x;
        float panelRight = panelCorners[2].x;
        float panelTop = panelCorners[1].y;
        float panelBottom = panelCorners[0].y;

        // Textのワールド空間でのコーナーを取得
        Vector3[] textCorners = new Vector3[4];
        textRect.GetWorldCorners(textCorners);

        // Textの範囲をチェック
        foreach (var corner in textCorners)
        {
            if (corner.x >= panelLeft && corner.x <= panelRight && corner.y >= panelBottom && corner.y <= panelTop)
            {
                return true; // 少なくとも1つのコーナーがPanel内
            }
        }

        return false; // すべてのコーナーがPanel外
    }

    void OnDestroy()
    {
        // Input Actionのクリーンアップ
        _aButtonAction.Disable();
    }
}
