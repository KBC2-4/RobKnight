using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class HintManager : MonoBehaviour
{
    public string hintMessage;   // ヒントのテキスト
    // public List<string> hints; // ヒントのリスト
    private TextMeshProUGUI _hintText;   // ヒントのテキストオブジェクト
    private GameObject _hintInstance; // ヒントのインスタンス
    private Material highlightMaterial;   // ハイライト用のマテリアル
    private Material originalMaterial;   // オリジナルのマテリアル
    private Renderer renderer;
    private Animator _animator;
    // private Animator _animator;
    private AsyncOperationHandle<Material> _materialHandle; // マテリアルHandle
    private AsyncOperationHandle<GameObject> _prefabHandle; // ヒント用のプレファブHandle
    private AsyncOperationHandle<Animator> _animatorHandle; // AnimatorのHandle


    void Start()
    {

        // ヒントUIのインスタンスを作成
        //_hintInstance = Instantiate(hintPrefab, transform);
        // 非表示にする


        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;

        // _animator = GetComponent<Animator>();
        // _animator.enabled = true;

        // マテリアルの読み込み
        LoadMaterial();
        // プレファブの読み込み
        LoadPrefab();
        // Animatorの読み込み
        //LoadAnimator();
    }

    void LoadMaterial()
    {
        // Addressables.LoadAssetAsync<Material>("Assets/Materials/UI/select.mat").Completed += OnMaterialLoaded;
        _materialHandle = Addressables.LoadAssetAsync<Material>("Assets/Materials/UI/select.mat");
        _materialHandle.Completed += OnMaterialLoaded;
    }

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

    void OnMaterialLoaded(AsyncOperationHandle<Material> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // Material loadedMaterial = handle.Result;
            highlightMaterial = handle.Result;
            // renderer.material = loadedMaterial;
        }
    }

    void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // プレファブのインスタンス化
            _hintInstance = Instantiate(handle.Result, transform.position, Quaternion.identity);
            _hintInstance.SetActive(false);
            _hintText = _hintInstance.GetComponentInChildren<TextMeshProUGUI>();
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


    void OnTriggerEnter(Collider other)
    {
        // プレイヤーが近づいたら
        if (other.tag == "Player")
        {
            // UIを表示
            ShowHint(hintMessage);
        }

        Highlight();
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーが離れたら
        if (other.tag == "Player")
        {
            // UIを非表示
            HideHint();
        }

        RemoveHighlight();
        // _animator.SetTrigger("isHide");
    }

    // ヒントを表示する
    public void ShowHint(string message)
    {
        _hintText.text = message;
        _hintInstance.SetActive(true);
        _animator.SetBool("isHide", false);
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
        _animator.SetBool("isHide", true);
        _hintInstance.SetActive(false);
    }

    public void Highlight()
    {
        renderer.material = highlightMaterial;
    }

    public void RemoveHighlight()
    {
        renderer.material = originalMaterial;
    }

    private void OnDestroy()
    {

        // マテリアルハンドルの解放
        if (_materialHandle.IsValid())
        {
            Addressables.Release(_materialHandle);
        }

        // プレファブハンドルの解放
        if (_prefabHandle.IsValid())
        {
            Addressables.Release(_prefabHandle);
        }
    }
}