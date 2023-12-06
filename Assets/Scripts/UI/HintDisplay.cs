using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

public class HintManager : MonoBehaviour
{
    [SerializeField, TextAreaAttribute(2, 5), Header("テキスト")] string hintMessage;   // ヒントのテキスト
    [SerializeField, Header("テキストの色")] Color textColor = Color.magenta; // テキスト色
    [SerializeField, Range(0f, 30f), Header("フォントサイズ")]
    private float fontSize = 20; // 初期フォントサイズを36に設定
    // public TMP_FontAsset font; // フォントアセット
    [SerializeField, Range(0f, 100f), Header("表示する距離")]
    private float range = 5.0f; // ヒントを表示する距離
    [SerializeField, Range(0f, 10f), Header("テキストの表示速度の間隔時間")] float _typingSpeed = 0.05f; // テキストの表示速度
    // テキストが表示し終わるまでプレイヤー操作を無効化するかのフラグ
    [SerializeField, Header("テキストが表示し終わるまでプレイヤー操作を無効化する")] private bool _disablePlayerInput = false;
    [SerializeField, Range(0f, 30f), Header("テキスト表示後のプレイヤーの待機時間（秒）")] private float _postDisplayWaitTime = 0f; // テキスト表示後のプレイヤーの待機時間（秒）
    [SerializeField, Header("1回のみ表示する")] private bool _displayOnceOnly = false; // 1回のみ表示するかのフラグ
    private bool _hasDisplayedText = false; // テキストが表示されたかどうかのフラグ
    private bool _isTyping = false; // テキストの表示中
    private int characterIndex = 0; // 表示した文字のインデックス
    // public List<string> hints; // ヒントのリスト
    private GameObject _player; // プレイヤーオブジェクト
    private PlayerController _playerController; // プレイヤーコントローラ
    private TextMeshProUGUI _hintText;   // ヒントのテキストオブジェクト
    private GameObject _hintInstance; // ヒントのインスタンス
    // private Material highlightMaterial;   // ハイライト用のマテリアル
    // private Material originalMaterial;   // オリジナルのマテリアル
    // private Renderer renderer;
    private Animator _animator;
    // private Animator _animator;
    // private AsyncOperationHandle<Material> _materialHandle; // マテリアルHandle
    private AsyncOperationHandle<GameObject> _prefabHandle; // ヒント用のプレファブHandle
    private AsyncOperationHandle<Animator> _animatorHandle; // AnimatorのHandle
    private Outline _outlineComponent;  // OutLineコンポーネント


    void Start()
    {
        _player = GameObject.FindWithTag("Player"); // プレイヤーの取得
        _playerController = _player.GetComponent<PlayerController>(); // プレイヤーコントローラの取得

        // ヒントUIのインスタンスを作成
        //_hintInstance = Instantiate(hintPrefab, transform);
        // 非表示にする


        // renderer = GetComponent<Renderer>();
        // originalMaterial = renderer.material;

        // _animator = GetComponent<Animator>();
        // _animator.enabled = true;

        // マテリアルの読み込み
        // LoadMaterial();
        // プレファブの読み込み
        LoadPrefab();
        // Animatorの読み込み
        //LoadAnimator();

        // Outlineコンポーネントの追加と初期化
        _outlineComponent = gameObject.AddComponent<Outline>();
        _outlineComponent.OutlineMode = Outline.Mode.OutlineVisible;
        _outlineComponent.OutlineColor = Color.white; // 好きな色に設定
        _outlineComponent.OutlineWidth = 5.0f;
        _outlineComponent.enabled = false; // 初期状態では無効
    }

    //void LoadMaterial()
    //{
    //    // Addressables.LoadAssetAsync<Material>("Assets/Materials/UI/select.mat").Completed += OnMaterialLoaded;
    //    _materialHandle = Addressables.LoadAssetAsync<Material>("Assets/Materials/UI/select.mat");
    //    _materialHandle.Completed += OnMaterialLoaded;
    //}

    void LoadPrefab()
    {
        _prefabHandle = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/UI/Canvas/HintCanvas.prefab");
        _prefabHandle.Completed += OnPrefabLoaded;
    }

    void LoadAnimator()
    {
        _animatorHandle = Addressables.LoadAssetAsync<Animator>("Assets/Animator/UI/HintCanvas.controller");
        _animatorHandle.Completed += OnAnimatorLoaded;
    }

    //void OnMaterialLoaded(AsyncOperationHandle<Material> handle)
    //{
    //    if (handle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        // Material loadedMaterial = handle.Result;
    //        highlightMaterial = handle.Result;
    //        // renderer.material = loadedMaterial;
    //    }
    //}

    void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // プレファブのインスタンス化
            // _hintInstance = Instantiate(handle.Result, transform.position, Quaternion.identity);
            // プレファブのインスタンス化。このスクリプトがアタッチされているオブジェクトを親とする
            _hintInstance = Instantiate(handle.Result, transform);
            _hintInstance.SetActive(false);
            _hintText = _hintInstance.GetComponentInChildren<TextMeshProUGUI>();
            _hintText.color = textColor; // テキストの色を設定
            _hintText.overflowMode = TextOverflowModes.Page; // オーバーフロー時のモードを設定
            _hintText.fontSize = fontSize; // フォントサイズを設定
            _animator = _hintInstance.GetComponent<Animator>();
        }
    }

    void OnAnimatorLoaded(AsyncOperationHandle<Animator> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _animator = handle.Result;
        }
    }

    void Update()
    {
        // Debug.Log(_hasDisplayedText);
        if (_displayOnceOnly && _hasDisplayedText)
        {
            return; // 1回のみ表示が有効で、テキストが既に表示されていたら、何もせずに戻る
        }

        float distance = Vector3.Distance(_player.transform.position, transform.position);

        if (distance <= range)
        {
            // if (!_hintInstance.activeSelf)
            {
                // UIを表示
                ShowHint(hintMessage);
                Highlight();
            }
        }
        else
        {
            // if (_hintInstance.activeSelf)
            {
                // UIを非表示
                HideHint();
                RemoveHighlight();
            }
        }
    }

    // ヒントを表示する
    public void ShowHint(string message)
    {

        // _hintText.text = message;
        if (!_isTyping){
            _isTyping = true;
            characterIndex = 0;
            _hintText.text = "";
            StartCoroutine(TypeText());
        }
        // _hintText.text = "";
        // StartCoroutine(TypeText());
        _hintInstance.SetActive(true);
        if (_animator != null)
        {
            _animator.SetBool("isVisible", true);
        }

        // 1回のみ表示が有効の場合
        if (_displayOnceOnly)
        {
            // テキストが表示されたとマーク
            _hasDisplayedText = true;
        }
    }

    IEnumerator TypeText(){
        // foreach (char character in hintMessage.toCharArray())
        // {
        //     _hintText.text += character;
        //     yield return new WaitForSeconds(typingSpeed);
        // }

        if (_postDisplayWaitTime > 0)
        {
            _playerController.SetInputAction(false); // プレイヤー操作を無効化
            StartCoroutine(Stay(_postDisplayWaitTime));
        }

        if (_disablePlayerInput)
        {
            _playerController.SetInputAction(false); // プレイヤー操作を無効化
        }


        foreach (char character in hintMessage)
        {
            _hintText.text += character;
            yield return new WaitForSeconds(_typingSpeed);
        }

        if (_disablePlayerInput)
        {
            _playerController.SetInputAction(true); // プレイヤー操作を有効化
        }

        //while (characterIndex < _hintText.text.Length){
        //    _hintText.text += _hintText.text[characterIndex++];
        //}
        //isTyping = false;
        //yield return new WaitForSeconds(typingSpeed);
    }

    // ヒントを表示する(複数対応)
    //public void ShowHint(int index)
    //{
    //    if (index >= 0 && index < hints.Count)
    //    {
    //        _hintText.text = hints[index];
    //    }
    //}

    IEnumerator Stay(float time)
    {
        yield return new WaitForSeconds(time);
        _playerController.SetInputAction(true); // プレイヤー操作を有効化
    }

    // ヒントを非表示にする
    public void HideHint()
    {
        if (_animator != null)
        {
            _animator.SetBool("isVisible", false);
            // _hintInstance.SetActive(false);
        }
        _isTyping = false;
    }

    // アニメーションイベントから呼ばれるメソッド
    public void DeactivateHint()
    {
        _hintInstance.SetActive(false);
    }

    public void Highlight()
    {
        // renderer.material = highlightMaterial;
        _outlineComponent.enabled = true; // 輪郭線を有効化
    }

    public void RemoveHighlight()
    {
        // renderer.material = originalMaterial;
        _outlineComponent.enabled = false; // 輪郭線を無効化
    }

    private void OnDestroy()
    {

        // マテリアルハンドルの解放
        //if (_materialHandle.IsValid())
        //{
        //    Addressables.Release(_materialHandle);
        //}

        // プレファブハンドルの解放
        if (_prefabHandle.IsValid())
        {
            Addressables.Release(_prefabHandle);
        }
    }
}