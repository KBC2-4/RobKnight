using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

public class GuideBarController : MonoBehaviour
{
    [SerializeField] private GameObject _guideCanvas;   // �K�C�h�p�̃L�����o�X
    [SerializeField] private GameObject keyboardMouseUI; // �L�[�{�[�h���}�E�X�p��UI�I�u�W�F�N�g
    [SerializeField] private GameObject gamepadUI; // �Q�[���R���g���[���[�p��UI�I�u�W�F�N�g
    [SerializeField] private GameObject touchUI; // ���o�C���p��UI�I�u�W�F�N�g
    private Animator _animator;
    private bool _isDisplay = false; // UI��\�����邩�ǂ����̃t���O
    [SerializeField] IntroCamera _introCamera; // �C���g���J����

    [SerializeField] private InputActionAsset _inputActions; // Input Action Asset�ւ̎Q��

    private float _noInputTimer = 0f;
    // private const float noInputThreshold = 1f; // ���͂��Ȃ��Ɣ��f���鎞��
    [SerializeField, Range(0f, 10f)] private float noInputThreshold = 3f; // ���͂��Ȃ��Ɣ��f���鎞��
    [SerializeField,Header("��ɕ\��")] private bool _isAlwayDisplay = false; // ��ɕ\�����邩�ǂ����̃t���O

    public static GuideBarController Instance { get; private set; } // �V���O���g���C���X�^���X

    public enum GuideName
    {
        Pause, // �|�[�Y
        Move,   // �v���C���[�ړ�
        Zoom,   // �~�j�}�b�v�g��k��
        Attack, // �U��
        Return, // �l�Ԃɖ߂�
        Possession, // �߈�
    }

    // �{�^���K�C�h�ƑΉ�����GameObject�̃}�b�s���O
    private Dictionary<GuideName, GameObject> _buttonGuides = new Dictionary<GuideName, GameObject>();

    // �K�C�h�̕\����Ԃ�ǐՂ��鎫��
    private Dictionary<GuideName, bool> _guideStates = new Dictionary<GuideName, bool>();


    /// <summary>
    /// �I�u�W�F�N�g�����߂ăA�N�e�B�u�ɂȂ������Ɉ�x�����Ăяo�����B
    /// �V���O���g���p�^�[�����������A�K�v�ɉ�����UI���\���ɂ��A�A�j���[�^��ݒ肷��B
    /// </summary>
    void Awake()
    {
        // �V���O���g���̎���
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);

            // �V�[�������[�h���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h��o�^
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        // Animator���擾����
        _animator = _guideCanvas.GetComponent<Animator>();
    }

    // �V�[�������[�h���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetUIForNewScene(scene.name);
    }

    // �V�����V�[���p��UI�����Z�b�g���郁�\�b�h
    private void ResetUIForNewScene(string sceneName)
    {
        // �^�C�g���V�[���̏ꍇ�́A�C���g���A�j���[�V�������Ȃ��̂ł��̂܂ܕ\������
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

        // UI���\���ɂ���
        _guideCanvas.SetActive(false);

        // �V�[�������擾
        string sceneName = SceneManager.GetActiveScene().name;

        // �^�C�g���V�[���̏ꍇ�́A�C���g���A�j���[�V�������Ȃ��̂ł��̂܂ܕ\������
        if (sceneName == "Title")
        {
            _isDisplay = true;
            _guideCanvas.SetActive(true);
            GuideSet(GuideName.Move,GuideName.Attack,GuideName.Possession);
                        // �K�C�h�̕\���E��\����Ԃ�K�p
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
    /// �w�肳�ꂽUI�p�l�����A�N�e�B�u�ȏꍇ�A����UI�p�l���Ɋ֘A����{�^���K�C�h�̃}�b�s���O���X�V����B
    /// </summary>
    /// <param name="activePanel">�A�N�e�B�u��UI�p�l��</param>
    private void UpdateGuides(GameObject activePanel)
    {

        //if (_activePanel.activeSelf)
        //{
        //    parentName = "UI/GuideCanvas" + _activePanel.name;
        //}

        // �e�I�u�W�F�N�g���܂߂ăA�N�e�B�u���m�F
        if (activePanel.activeInHierarchy)
        {

            // �e�{�^���K�C�h�ƑΉ�����GameObject���}�b�s���O
            //_buttonGuides.Add("Attack", activePanel.transform.Find("AttackAction").gameObject);
            //_buttonGuides.Add("Return", activePanel.transform.Find("ReturnAction").gameObject);
            //_buttonGuides.Add("Possession", activePanel.transform.Find("PossessionAction").gameObject);

            // �e�{�^���K�C�h�ƑΉ�����GameObject���}�b�s���O�i�����̃L�[������Ώ㏑���j
            _buttonGuides[GuideName.Pause] = activePanel.transform.Find("PauseAction").gameObject;
            _buttonGuides[GuideName.Move] = activePanel.transform.Find("MoveAction").gameObject;
            _buttonGuides[GuideName.Zoom] = activePanel.transform.Find("ZoomAction").gameObject;
            _buttonGuides[GuideName.Attack] = activePanel.transform.Find("AttackAction").gameObject;
            _buttonGuides[GuideName.Return] = activePanel.transform.Find("ReturnAction").gameObject;
            _buttonGuides[GuideName.Possession] = activePanel.transform.Find("PossessionAction").gameObject;

            // ������Ԃł͂��ׂẴ{�^���K�C�h���\���ɂ���
            //foreach (var guide in _buttonGuides.Values)
            //{
            //    Debug.Log(guide.name);
            //    guide.SetActive(false);
            //}

            // �K�C�h�̕\���E��\����Ԃ�K�p
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
    /// �I�u�W�F�N�g���A�N�e�B�u�ɂȂ������ɌĂяo�����B
    /// �A�j���[�V�����̐ݒ�A���̓A�N�V�����̗L�����A�f�o�C�X�^�C�v�̕ύX�C�x���g�̓o�^���s���B
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
    /// �I�u�W�F�N�g����A�N�e�B�u�ɂȂ������ɌĂяo�����B
    /// �A�j���[�V�����̐ݒ�A���̓A�N�V�����̖������A�f�o�C�X�^�C�v�̕ύX�C�x���g�̉������s���B
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
    /// �C���g���A�j���[�V���������������ۂɌĂяo�����n���h���[�B���̊֐��ł�UI��\����Ԃɐ؂�ւ��܂��B
    /// �C���g���A�j���[�V�����̊��������m���āAUI�\���t���O��true�ɐݒ肵�A�K�C�h�L�����o�X���A�N�e�B�u�ɂ��܂��B
    /// </summary>
    private void HandleIntroAnimationComplete()
    {
        // �C���g���A�j���[�V�����������UI��\��
        _isDisplay = true;
        _guideCanvas.SetActive(true);
        // SetUIVisibility();
    }

    // <summary>
    // �w�肳�ꂽUI���A�N�e�B�u�ɂ��A����UI���A�N�e�B�u�ɂ���
    // </summary>
    // <param name="activePanel">�A�N�e�B�u��UI</param>
    private void SetActivePanel(GameObject activePanel)
    {
        keyboardMouseUI.SetActive(activePanel == keyboardMouseUI);
        gamepadUI.SetActive(activePanel == gamepadUI);
        touchUI.SetActive(activePanel == touchUI);

        // �{�^���K�C�h�̏�����
        UpdateGuides(activePanel);
    }

    /// <summary>
    /// ���݂̓��̓f�o�C�X�Ɋ�Â��ēK�؂�UI�p�l�����A�N�e�B�u�ɂ���B
    /// </summary>
    /// <param name="device">InputDevice</param>
    private void UpdateUI(InputDevice device)
    {

        if (device is Keyboard || device is Mouse)
        {
            // �L�[�{�[�h�ƃ}�E�X�p��UI�ɐ؂�ւ�
            // keyboardMouseUI.SetActive(_isDisplay);
            SetActivePanel(keyboardMouseUI);
        }
        else if (device is Gamepad)
        {
            // �Q�[���p�b�h�p��UI�ɐ؂�ւ�
            // gamepadUI.SetActive(_isDisplay);
            SetActivePanel(gamepadUI);
        }
        else if (device is Touchscreen)
        {
            // �^�b�`�X�N���[���p��UI�ɐ؂�ւ�
            // touchUI.SetActive(_isDisplay);
            SetActivePanel(touchUI);
        }
    }


    /// <summary>
    /// ���t���[���Ăяo����A���̗͂L���Ɋ�Â���UI�̕\���𐧌䂷��B
    /// </summary>
    private void Update()
    {

        if (IsAnyInput())
        {
            // �������͂��������Ƃ�
            _noInputTimer = 0f;
            _animator.SetBool("IsDisplay", false);
            // _animator.SetBool("IsDisplay", _isDisplay);
            // _guideCanvas.SetActive(false);
            _isDisplay = false;
        }
        else
        {
            // ���͂��Ȃ��ꍇ�A�^�C�}�[�𑝉�
            _noInputTimer += Time.deltaTime;

            if (_noInputTimer >= noInputThreshold)
            {
                // �w�莞�Ԉȏ���͂��Ȃ��ꍇ�AUI��\��
                _animator.SetBool("IsDisplay", true);
                // _guideCanvas.SetActive(true);
                _isDisplay = true;
            }
        }
    }

    /// <summary>
    /// �C�ӂ̃L�[�{�[�h�A�}�E�X�A�Q�[���p�b�h�̓��͂����邩�ǂ������m�F����B
    /// �g�p��F���͂�����ꍇ��UI���\���ɂ��A�Ȃ��ꍇ�͕\�����邽�߂̔��f�Ɏg�p�B
    /// </summary>
    /// <returns>���͂�����ꍇ��true�A����ȊO��false</returns>
    private bool IsAnyInput()
    {
        // ��ɕ\���t���O�������Ă���ꍇ�͓��͂����m���Ȃ�
        if (_isAlwayDisplay)
        {
            return false;
        }

        // �L�[�{�[�h�A�}�E�X�A�Q�[���p�b�h�̓��͂��`�F�b�N
        return Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Gamepad.current?.allControls.Any(control => control.IsPressed()) == true;
    }

    /// <summary>
    /// �w�肳�ꂽ���O�̃K�C�h���A�N�e�B�u�ɂ���B
    /// �g�p��F����̑��삪�K�v�ȏ�ʂŁA�֘A���鑀��K�C�h��\�����邽�߂Ɏg�p�B
    /// </summary>
    /// <param name="guideName">�A�N�e�B�u�ɂ���K�C�h�̖��O�iGuideName enum�j</param>
    public void AddGuide(GuideName guideName)
    {
        if (_buttonGuides.TryGetValue(guideName, out GameObject guide))
        {
            guide.SetActive(true);
            _guideStates[guideName] = true;
        }
    }

    /// <summary>
    /// �w�肳�ꂽ���O�̃K�C�h���A�N�e�B�u�ɂ���B
    /// �g�p��F����̑��삪�s�v�ɂȂ�����ʂŁA�֘A���鑀��K�C�h���\���ɂ��邽�߂Ɏg�p�B
    /// </summary>
    /// <param name="guideName">��A�N�e�B�u�ɂ���K�C�h�̖��O�iGuideName enum�j</param>
    public void RemoveGuide(GuideName guideName)
    {
        if (_buttonGuides.TryGetValue(guideName, out GameObject guide))
        {
            guide.SetActive(false);
            _guideStates[guideName] = false;
        }
    }

    /// <summary>
    /// �w�肳�ꂽ���O�̃K�C�h�݂̂��A�N�e�B�u�ɂ��A�����A�N�e�B�u�ɂ���B
    /// �g�p��F�����̑��삪�K�v�ȃV�i���I�ŁA�֘A���鑀��K�C�h�݂̂���x�ɕ\�����邽�߂Ɏg�p�B
    /// </summary>
    /// <param name="guideNames">�A�N�e�B�u�ɂ���K�C�h�̖��O�̃Z�b�g�iGuideName enum�̔z��j</param>
    public void GuideSet(params GuideName[] guideNames)
    {
        //// ���ׂẴK�C�h���\���ɂ���
        //foreach (var guide in _buttonGuides.Values)
        //{
        //    guide.SetActive(false);
        //}

        //// �w�肳�ꂽ�K�C�h�݂̂�\������
        //foreach (var guideName in guideNames)
        //{
        //    AddGuide(guideName);
        //}

        // ���ׂẴK�C�h���\���ɂ��A��Ԃ��X�V
        foreach (var guidePair in _buttonGuides)
        {
            guidePair.Value.SetActive(false);
            _guideStates[guidePair.Key] = false;
        }

        // �w�肳�ꂽ�K�C�h�݂̂�\�����A��Ԃ��X�V
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
