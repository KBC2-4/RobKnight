using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

public class HintManager : MonoBehaviour
{
    [SerializeField, TextAreaAttribute(2, 5), Header("�e�L�X�g")] string hintMessage;   // �q���g�̃e�L�X�g
    [SerializeField, Header("�e�L�X�g�̐F")] Color textColor = Color.magenta; // �e�L�X�g�F
    [SerializeField, Range(0f, 30f), Header("�t�H���g�T�C�Y")]
    private float fontSize = 20; // �����t�H���g�T�C�Y��36�ɐݒ�
    // public TMP_FontAsset font; // �t�H���g�A�Z�b�g
    [SerializeField, Range(0f, 100f), Header("�\�����鋗��")]
    private float range = 5.0f; // �q���g��\�����鋗��
    [SerializeField, Range(0f, 10f), Header("�e�L�X�g�̕\�����x�̊Ԋu����")] float _typingSpeed = 0.05f; // �e�L�X�g�̕\�����x
    // �e�L�X�g���\�����I���܂Ńv���C���[����𖳌������邩�̃t���O
    [SerializeField, Header("�e�L�X�g���\�����I���܂Ńv���C���[����𖳌�������")] private bool _disablePlayerInput = false;
    [SerializeField, Range(0f, 30f), Header("�e�L�X�g�\����̃v���C���[�̑ҋ@���ԁi�b�j")] private float _postDisplayWaitTime = 0f; // �e�L�X�g�\����̃v���C���[�̑ҋ@���ԁi�b�j
    [SerializeField, Header("1��̂ݕ\������")] private bool _displayOnceOnly = false; // 1��̂ݕ\�����邩�̃t���O
    private bool _hasDisplayedText = false; // �e�L�X�g���\�����ꂽ���ǂ����̃t���O
    private bool _isTyping = false; // �e�L�X�g�̕\����
    private int characterIndex = 0; // �\�����������̃C���f�b�N�X
    // public List<string> hints; // �q���g�̃��X�g
    private GameObject _player; // �v���C���[�I�u�W�F�N�g
    private PlayerController _playerController; // �v���C���[�R���g���[��
    private TextMeshProUGUI _hintText;   // �q���g�̃e�L�X�g�I�u�W�F�N�g
    private GameObject _hintInstance; // �q���g�̃C���X�^���X
    // private Material highlightMaterial;   // �n�C���C�g�p�̃}�e���A��
    // private Material originalMaterial;   // �I���W�i���̃}�e���A��
    // private Renderer renderer;
    private Animator _animator;
    // private Animator _animator;
    // private AsyncOperationHandle<Material> _materialHandle; // �}�e���A��Handle
    private AsyncOperationHandle<GameObject> _prefabHandle; // �q���g�p�̃v���t�@�uHandle
    private AsyncOperationHandle<Animator> _animatorHandle; // Animator��Handle
    private Outline _outlineComponent;  // OutLine�R���|�[�l���g


    void Start()
    {
        _player = GameObject.FindWithTag("Player"); // �v���C���[�̎擾
        _playerController = _player.GetComponent<PlayerController>(); // �v���C���[�R���g���[���̎擾

        // �q���gUI�̃C���X�^���X���쐬
        //_hintInstance = Instantiate(hintPrefab, transform);
        // ��\���ɂ���


        // renderer = GetComponent<Renderer>();
        // originalMaterial = renderer.material;

        // _animator = GetComponent<Animator>();
        // _animator.enabled = true;

        // �}�e���A���̓ǂݍ���
        // LoadMaterial();
        // �v���t�@�u�̓ǂݍ���
        LoadPrefab();
        // Animator�̓ǂݍ���
        //LoadAnimator();

        // Outline�R���|�[�l���g�̒ǉ��Ə�����
        _outlineComponent = gameObject.AddComponent<Outline>();
        _outlineComponent.OutlineMode = Outline.Mode.OutlineVisible;
        _outlineComponent.OutlineColor = Color.white; // �D���ȐF�ɐݒ�
        _outlineComponent.OutlineWidth = 5.0f;
        _outlineComponent.enabled = false; // ������Ԃł͖���
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
            // �v���t�@�u�̃C���X�^���X��
            // _hintInstance = Instantiate(handle.Result, transform.position, Quaternion.identity);
            // �v���t�@�u�̃C���X�^���X���B���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g��e�Ƃ���
            _hintInstance = Instantiate(handle.Result, transform);
            _hintInstance.SetActive(false);
            _hintText = _hintInstance.GetComponentInChildren<TextMeshProUGUI>();
            _hintText.color = textColor; // �e�L�X�g�̐F��ݒ�
            _hintText.overflowMode = TextOverflowModes.Page; // �I�[�o�[�t���[���̃��[�h��ݒ�
            _hintText.fontSize = fontSize; // �t�H���g�T�C�Y��ݒ�
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
            return; // 1��̂ݕ\�����L���ŁA�e�L�X�g�����ɕ\������Ă�����A���������ɖ߂�
        }

        float distance = Vector3.Distance(_player.transform.position, transform.position);

        if (distance <= range)
        {
            // if (!_hintInstance.activeSelf)
            {
                // UI��\��
                ShowHint(hintMessage);
                Highlight();
            }
        }
        else
        {
            // if (_hintInstance.activeSelf)
            {
                // UI���\��
                HideHint();
                RemoveHighlight();
            }
        }
    }

    // �q���g��\������
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

        // 1��̂ݕ\�����L���̏ꍇ
        if (_displayOnceOnly)
        {
            // �e�L�X�g���\�����ꂽ�ƃ}�[�N
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
            _playerController.SetInputAction(false); // �v���C���[����𖳌���
            StartCoroutine(Stay(_postDisplayWaitTime));
        }

        if (_disablePlayerInput)
        {
            _playerController.SetInputAction(false); // �v���C���[����𖳌���
        }


        foreach (char character in hintMessage)
        {
            _hintText.text += character;
            yield return new WaitForSeconds(_typingSpeed);
        }

        if (_disablePlayerInput)
        {
            _playerController.SetInputAction(true); // �v���C���[�����L����
        }

        //while (characterIndex < _hintText.text.Length){
        //    _hintText.text += _hintText.text[characterIndex++];
        //}
        //isTyping = false;
        //yield return new WaitForSeconds(typingSpeed);
    }

    // �q���g��\������(�����Ή�)
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
        _playerController.SetInputAction(true); // �v���C���[�����L����
    }

    // �q���g���\���ɂ���
    public void HideHint()
    {
        if (_animator != null)
        {
            _animator.SetBool("isVisible", false);
            // _hintInstance.SetActive(false);
        }
        _isTyping = false;
    }

    // �A�j���[�V�����C�x���g����Ă΂�郁�\�b�h
    public void DeactivateHint()
    {
        _hintInstance.SetActive(false);
    }

    public void Highlight()
    {
        // renderer.material = highlightMaterial;
        _outlineComponent.enabled = true; // �֊s����L����
    }

    public void RemoveHighlight()
    {
        // renderer.material = originalMaterial;
        _outlineComponent.enabled = false; // �֊s���𖳌���
    }

    private void OnDestroy()
    {

        // �}�e���A���n���h���̉��
        //if (_materialHandle.IsValid())
        //{
        //    Addressables.Release(_materialHandle);
        //}

        // �v���t�@�u�n���h���̉��
        if (_prefabHandle.IsValid())
        {
            Addressables.Release(_prefabHandle);
        }
    }
}