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
    
        if (device is Keyboard || device is Mouse)
        {
            Debug.Log("Keyboard & Mouse input detected");
            NotifyDeviceTypeChanged(device);
        }
        else if (device is Gamepad)
        {
            Debug.Log("Gamepad input detected");
            NotifyDeviceTypeChanged(device);
        }
        else if (device is Touchscreen)
        {
            Debug.Log("Touch input detected");
            NotifyDeviceTypeChanged(device);
        }
    }
    
    private static void NotifyDeviceTypeChanged(InputDevice device)
    {
        OnDeviceTypeChanged?.Invoke(device);
    }
}