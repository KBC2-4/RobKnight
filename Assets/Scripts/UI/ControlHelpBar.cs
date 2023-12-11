using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

public class ControlHelpBar : MonoBehaviour
{
    [SerializeField] private GameObject _guideCanvas;   // �K�C�h�p�̃L�����o�X
    [SerializeField] private GameObject keyboardMouseUI; // �L�[�{�[�h���}�E�X�p��UI�I�u�W�F�N�g
    [SerializeField] private GameObject gamepadUI; // �Q�[���R���g���[���[�p��UI�I�u�W�F�N�g
    [SerializeField] private GameObject touchUI; // ���o�C���p��UI�I�u�W�F�N�g
    private Animator _animator;
    // public GameObject uiPrompt;
    private InputDevice lastActiveDevice; // �O��A�N�e�B�u�ɂȂ����f�o�C�X
    private bool _isDisplay = false; // UI��\�����邩�ǂ����̃t���O
    [SerializeField] IntroCamera _introCamera; // �C���g���J����

    [SerializeField] private InputActionAsset _inputActions; // Input Action Asset�ւ̎Q��

    private float _noInputTimer = 0f;
    private const float noInputThreshold = 3f; // ���͂��Ȃ��Ɣ��f���鎞��

    public static ControlHelpBar Instance { get; private set; }

    private Dictionary<string, GameObject> _buttonGuides = new Dictionary<string, GameObject>();


    void Awake()
    {
        // �V���O���g���̎���
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // �{�^���K�C�h�̏�����
        InitializeGuides();

        _guideCanvas.GetComponent<Animator>();


        // UI���\���ɂ���
        _guideCanvas.SetActive(false);

        // Animator���擾����
        _animator = _guideCanvas.GetComponent<Animator>();

        // �ꎞ
        //keyboardMouseUI.SetActive(false);
        //gamepadUI.SetActive(false);
        //touchUI.SetActive(false);

        // UI��\���ɂ���
        _guideCanvas.SetActive(true);
        keyboardMouseUI.SetActive(true);
        gamepadUI.SetActive(true);
        touchUI.SetActive(true);

        // �V�[�������擾
        string sceneName = SceneManager.GetActiveScene().name;

        // �^�C�g���V�[���̏ꍇ�́A�C���g���A�j���[�V�������Ȃ��̂ł��̂܂ܕ\������
        if (sceneName == "Title")
        {
            _isDisplay = true;
            _guideCanvas.SetActive(true);
            gamepadUI.SetActive(true);
            // SetUIVisibility();
        }
    }

    private void InitializeGuides()
    {
        // �e�{�^���K�C�h�ƑΉ�����GameObject���}�b�s���O
        //_buttonGuides.Add("Attack", attackButtonUI);
        //_buttonGuides.Add("Jump", jumpButtonUI);
        //_buttonGuides.Add("Guard", guardButtonUI);

        // ������Ԃł͂��ׂẴ{�^���K�C�h���\���ɂ���
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
        // �C���g���A�j���[�V�����������UI��\��
        _isDisplay = true;
        _guideCanvas.SetActive(true);
        // gamepadUI.SetActive(true);
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
        // �f�o�C�X�^�C�v�Ɋ�Â���UI�������ݒ�
        //keyboardMouseUI.SetActive(device is Keyboard || device is Mouse);
        //gamepadUI.SetActive(device is Gamepad);
        //touchUI.SetActive(device is Touchscreen);

        // �f�o�C�X�^�C�v�Ɋ�Â���UI��\���E��\��
        if (Keyboard.current != null || Mouse.current != null)
        {
            keyboardMouseUI.SetActive(_isDisplay);
        }
        else if (Gamepad.current != null)
        {
            gamepadUI.SetActive(_isDisplay);
        }
        else if (Touchscreen.current != null)
        {
            touchUI.SetActive(_isDisplay);
        }


        //bool isKeyboardMouse = device is Keyboard || device is Mouse;
        //bool isGamepad = device is Gamepad;
        //bool isTouchscreen = device is Touchscreen;

        //if (_keyboardMouseUIAnimator != null)
        //{
        //    _keyboardMouseUIAnimator.SetBool("IsDisplay", isKeyboardMouse && _isDisplay);
        //}

        //if (_gamepadUIAnimator != null)
        //{
        //    _gamepadUIAnimator.SetBool("IsDisplay", isGamepad && _isDisplay);
        //}

        //if (_touchUIAnimator != null)
        //{
        //    _touchUIAnimator.SetBool("IsDisplay", isTouchscreen && _isDisplay);
        //}
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    // �v���C���[���߂Â�����
    //    if (other.tag == "Player")
    //    {
    //        // UI��\��
    //        // _isDisplay = true;
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    // �v���C���[�����ꂽ��
    //    if (other.tag == "Player")
    //    {
    //        // UI���\��
    //        // _isDisplay = false;
    //    }
    //}

    private void Update()
    {
        // UpdateUIBasedOnInputDevice();

        if (IsAnyInput())
        {
            // �������͂��������Ƃ�
            _noInputTimer = 0f;
            _guideCanvas.SetActive(false);
            _isDisplay = false;
        }
        else
        {
            // ���͂��Ȃ��ꍇ�A�^�C�}�[�𑝉�
            _noInputTimer += Time.deltaTime;

            if (_noInputTimer >= noInputThreshold)
            {
                // �w�莞�Ԉȏ���͂��Ȃ��ꍇ�AUI��\��
                _guideCanvas.SetActive(true);
                _isDisplay = true;
            }
        }
    }

    private bool IsAnyInput()
    {
        // �L�[�{�[�h�A�}�E�X�A�Q�[���p�b�h�̓��͂��`�F�b�N
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
        // ���ׂẴK�C�h���\���ɂ���
        foreach (var guide in _buttonGuides.Values)
        {
            guide.SetActive(false);
        }

        // �w�肳�ꂽ�K�C�h�݂̂�\������
        foreach (var guideName in guideNames)
        {
            AddGuide(guideName);
        }
    }
}
