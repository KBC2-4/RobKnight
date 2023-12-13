using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CredittController : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 20f;
    [SerializeField] private float _fastForwardSpeed = 100f; // ������̃X�s�[�h
    [SerializeField] private TMP_Text _creditsText;
    [SerializeField] private GameObject _creditsPanel;
    private RectTransform _creditsPanelRect;
    private Animator _creditsPanelAnimator;
    private InputAction _aButtonAction; // A�{�^���p��Input Action
    private InputControls _inputControls; // Input Actions�N���X�̃C���X�^���X

    void Awake()
    {
        _inputControls = new InputControls();
    }

    void OnEnable()
    {
        // Input Actions��L����
        _inputControls.Enable();
    }

    void OnDisable()
    {
        // Input Actions�𖳌���
        _inputControls.Disable();
    }

    void Start()
    {
        _creditsPanel.SetActive(true);
        _creditsPanelAnimator = _creditsPanel.GetComponent<Animator>();

        // Input Action�̏�����
        _aButtonAction = new InputAction(binding: "<Gamepad>/buttonSouth");
        _aButtonAction.Enable();

        // _creditsPanel����RectTransform�R���|�[�l���g���擾
        _creditsPanelRect = _creditsPanel.GetComponent<RectTransform>();

        if (_creditsPanelRect == null)
        {
            // �G���[�n���h�����O
            Debug.LogError("_creditsPanel��RectTransform�R���|�[�l���g��������܂���ł����B");
        }
    }

    void Update()
    {
        // �e�L�X�g����ɃX�N���[��
        // creditsText.transform.position = new Vector2(_creditsText.transform.position.x, _creditsText.transform.position.y + _scrollSpeed * Time.deltaTime);

        // A�{�^����������Ă��邩�`�F�b�N
        //bool isAFastForwarding = _aButtonAction.ReadValue<float>() > 0;

        //// �����肪�A�N�e�B�u�̏ꍇ�͑��x�𑝉�������
        //float currentSpeed = isAFastForwarding ? _fastForwardSpeed : _scrollSpeed;

        // Submit�A�N�V�����̏�Ԃ��m�F
        bool isSubmitting = _inputControls.UI.Submit.ReadValue<float>() > 0;

        // �����肪�A�N�e�B�u�̏ꍇ�͑��x�𑝉�������
        float currentSpeed = isSubmitting ? _fastForwardSpeed : _scrollSpeed;

        // �e�L�X�g����ɃX�N���[��
        _creditsText.transform.position = new Vector2(_creditsText.transform.position.x, _creditsText.transform.position.y + currentSpeed * Time.deltaTime);

        // �A�j���[�V�����̃g���K�[�i����̈ʒu�Ńt�F�[�h�C�����J�n�j
        if (_creditsText.transform.position.y >= 1000)
        {
            // _creditsPanelAnimator.SetTrigger("FadeIn");
        }

        // ��������ʊO�ɏo����Title�V�[���ɑJ��
        //if (_creditsText.transform.position.y >= 1000) // ���̒l�͓K�X����
        //{
        //    SceneManager.LoadScene("Title"); // �^�C�g���V�[���ɑJ��
        //}
        // Text��Panel�͈̔͊O�ɏo�����m�F
        if (!IsTextWithinPanel(_creditsText.rectTransform, _creditsPanelRect))
        {
            Debug.Log("�͈͊O�ɏo�܂���");
            SceneManager.LoadScene("Title"); // �^�C�g���V�[���ɑJ��
        }
    }

    // Text��Panel�͈͓̔��ɂ��邩�ǂ����𔻒f���郁�\�b�h
    private bool IsTextWithinPanel(RectTransform textRect, RectTransform panelRect)
    {
        // Panel�̃��[���h��Ԃł̃R�[�i�[���擾
        Vector3[] panelCorners = new Vector3[4];
        panelRect.GetWorldCorners(panelCorners);

        // Panel�͈̔͂��`
        float panelLeft = panelCorners[0].x;
        float panelRight = panelCorners[2].x;
        float panelTop = panelCorners[1].y;
        float panelBottom = panelCorners[0].y;

        // Text�̃��[���h��Ԃł̃R�[�i�[���擾
        Vector3[] textCorners = new Vector3[4];
        textRect.GetWorldCorners(textCorners);

        // Text�͈̔͂��`�F�b�N
        foreach (var corner in textCorners)
        {
            if (corner.x >= panelLeft && corner.x <= panelRight && corner.y >= panelBottom && corner.y <= panelTop)
            {
                return true; // ���Ȃ��Ƃ�1�̃R�[�i�[��Panel��
            }
        }

        return false; // ���ׂẴR�[�i�[��Panel�O
    }

    void OnDestroy()
    {
        // Input Action�̃N���[���A�b�v
        _aButtonAction.Disable();
    }
}
