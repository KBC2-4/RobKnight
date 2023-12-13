using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceTypeDetector : MonoBehaviour
{
    // �V���O���g���C���X�^���X
    public static DeviceTypeDetector Instance { get; private set; }
    
    // �f�o�C�X�^�C�v���ύX���ꂽ�Ƃ��Ƀg���K�[�����C�x���g
    public delegate void DeviceTypeChanged(InputDevice device);
    public static event DeviceTypeChanged OnDeviceTypeChanged;
    
    public InputAction keyboardMouseAction; // �L�[�{�[�h�ƃ}�E�X�̓��͂����o����A�N�V����
    public InputAction gamepadAction; // �Q�[���p�b�h�̓��͂����o����A�N�V����
    public InputAction touchAction; // �^�b�`�p�b�h�̓��͂����o����A�N�V����

    private InputDevice _currentDevice; // ���݂̃A�N�e�B�u�ȃf�o�C�X

    private void Awake()
    {
        // �V���O���g���̏�����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // �Q�[���J�n���Ƀf�o�C�X�^�C�v�𔻒f
        // �R���[�`�����J�n
        StartCoroutine(DetectInitialDeviceType());
    }

    // �Q�[���J�n���ɗ��p�\�ȃf�o�C�X�����o���郁�\�b�h
    // �����f�o�C�X�^�C�v�����o����R���[�`��
    private IEnumerator DetectInitialDeviceType()
    {
        // Input System���f�o�C�X������������܂őҋ@
        yield return new WaitUntil(() => InputSystem.devices.Count > 0);

        var devices = InputSystem.devices;
        InputDevice initialDevice = null;

        // �ŏ��Ɍ��o���ꂽ�f�o�C�X�����o
        //if (devices.Any(device => device is Keyboard || device is Mouse))
        //{
        //    initialDevice = devices.First(device => device is Keyboard || device is Mouse);
        //}
        //else if (devices.Any(device => device is Gamepad))
        //{
        //    initialDevice = devices.First(device => device is Gamepad);
        //}

        // �Q�[���p�b�h��D��I�Ɍ��o
        // �Q�[���p�b�h���ŏ��Ƀ`�F�b�N
        if (devices.Any(device => device is Gamepad))
        {
            initialDevice = devices.First(device => device is Gamepad);
        }
        // �L�[�{�[�h�E�}�E�X�����Ƀ`�F�b�N
        else if (devices.Any(device => device is Keyboard || device is Mouse))
        {
            initialDevice = devices.First(device => device is Keyboard || device is Mouse);
        }

        // �����f�o�C�X�����o���ꂽ�ꍇ�A�C�x���g���g���K�[
        if (initialDevice != null)
        {
            // ���݂̃f�o�C�X���X�V
            _currentDevice = initialDevice;
            NotifyDeviceTypeChanged(initialDevice);
        }
    }

    private void OnEnable()
    {
        // �A�N�V�����̎��s���Ƀf�o�C�X�̎�ނ𔻕ʂ���C�x���g�����ꂼ��o�^
        keyboardMouseAction.performed += ctx => DetectDeviceType(ctx);
        gamepadAction.performed += ctx => DetectDeviceType(ctx);
        touchAction.performed += ctx => DetectDeviceType(ctx);

        keyboardMouseAction.Enable();
        gamepadAction.Enable();
        touchAction.Enable();
    }

    private void OnDisable()
    {
        keyboardMouseAction.Disable();
        gamepadAction.Disable();
        touchAction.Disable();
    }
    
    private void DetectDeviceType(InputAction.CallbackContext context)
    {
        // ���̓f�o�C�X���擾
        var device = context.control.device;

        // ���݂̃f�o�C�X���X�V
        _currentDevice = device;

        if (device is Keyboard || device is Mouse)
        {
            // Debug.Log("Keyboard & Mouse input detected");
            NotifyDeviceTypeChanged(device);
        }
        else if (device is Gamepad)
        {
            // Debug.Log("Gamepad input detected");
            NotifyDeviceTypeChanged(device);
        }
        else if (device is Touchscreen)
        {
            // Debug.Log("Touch input detected");
            NotifyDeviceTypeChanged(device);
        }
    }

    /// <summary>
    /// ���݂̃f�o�C�X��Ԃ����\�b�h
    /// </summary>
    /// <returns></returns>
    public InputDevice GetCurrentDevice()
    {
        return _currentDevice;
    }

    private static void NotifyDeviceTypeChanged(InputDevice device)
    {
        OnDeviceTypeChanged?.Invoke(device);
    }
}