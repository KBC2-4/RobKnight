using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class HintManager : MonoBehaviour
{
    public string hintMessage;   // �q���g�̃e�L�X�g
    // public List<string> hints; // �q���g�̃��X�g
    private TextMeshProUGUI _hintText;   // �q���g�̃e�L�X�g�I�u�W�F�N�g
    private GameObject _hintInstance; // �q���g�̃C���X�^���X
    private Material highlightMaterial;   // �n�C���C�g�p�̃}�e���A��
    private Material originalMaterial;   // �I���W�i���̃}�e���A��
    private Renderer renderer;
    private Animator _animator;
    // private Animator _animator;
    private AsyncOperationHandle<Material> _materialHandle; // �}�e���A��Handle
    private AsyncOperationHandle<GameObject> _prefabHandle; // �q���g�p�̃v���t�@�uHandle
    private AsyncOperationHandle<Animator> _animatorHandle; // Animator��Handle


    void Start()
    {

        // �q���gUI�̃C���X�^���X���쐬
        //_hintInstance = Instantiate(hintPrefab, transform);
        // ��\���ɂ���


        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;

        // _animator = GetComponent<Animator>();
        // _animator.enabled = true;

        // �}�e���A���̓ǂݍ���
        LoadMaterial();
        // �v���t�@�u�̓ǂݍ���
        LoadPrefab();
        // Animator�̓ǂݍ���
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
            // �v���t�@�u�̃C���X�^���X��
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
        // �v���C���[���߂Â�����
        if (other.tag == "Player")
        {
            // UI��\��
            ShowHint(hintMessage);
        }

        Highlight();
    }

    void OnTriggerExit(Collider other)
    {
        // �v���C���[�����ꂽ��
        if (other.tag == "Player")
        {
            // UI���\��
            HideHint();
        }

        RemoveHighlight();
        // _animator.SetTrigger("isHide");
    }

    // �q���g��\������
    public void ShowHint(string message)
    {
        _hintText.text = message;
        _hintInstance.SetActive(true);
        _animator.SetBool("isHide", false);
    }

    // �q���g��\������(�����Ή�)
    //public void ShowHint(int index)
    //{
    //    if (index >= 0 && index < hints.Count)
    //    {
    //        _hintText.text = hints[index];
    //    }
    //}

    // �q���g���\���ɂ���
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

        // �}�e���A���n���h���̉��
        if (_materialHandle.IsValid())
        {
            Addressables.Release(_materialHandle);
        }

        // �v���t�@�u�n���h���̉��
        if (_prefabHandle.IsValid())
        {
            Addressables.Release(_prefabHandle);
        }
    }
}