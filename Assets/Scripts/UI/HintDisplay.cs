using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

public class HintManager : MonoBehaviour
{
    public string hintMessage;   // ヒントのテキスト
    public Color textColor = Color.magenta; // テキスト色
    [SerializeField, Range(0f, 30f)]
    private float fontSize = 20; // 初期フォントサイズを36に設定
    // public TMP_FontAsset font; // フォントアセット
    [SerializeField, Range(0f, 100f)]
    private float range = 5.0f; // ヒントを表示する距離
    [SerializeField] float typingSpeed = 0.05f; // テキストの表示速度
    private bool isTyping = false; // テキストの表示中
    private int characterIndex = 0; // 表示した文字のインデックス
    // public List<string> hints; // ヒントのリスト
    private GameObject player; // プレイヤーオブジェクト
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
        player = GameObject.FindWithTag("Player"); // プレイヤーの取得

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
        float distance = Vector3.Distance(player.transform.position, transform.position);

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
        if(!isTyping){
            isTyping = true;
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

    }

    IEnumerator TypeText(){
        // foreach (char character in hintMessage.toCharArray())
        // {
        //     _hintText.text += character;
        //     yield return new WaitForSeconds(typingSpeed);
        // }
        foreach (char character in hintMessage)
        {
            _hintText.text += character;
            yield return new WaitForSeconds(typingSpeed);
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

    // ヒントを非表示にする
    public void HideHint()
    {
        if (_animator != null)
        {
            _animator.SetBool("isVisible", false);
            // _hintInstance.SetActive(false);
        }
        isTyping = false;
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