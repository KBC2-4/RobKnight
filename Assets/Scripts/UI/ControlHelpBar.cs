using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class ControlHelpBar : MonoBehaviour
{
    [SerializeField] private GameObject keyboardMouseUI; // �L�[�{�[�h���}�E�X�p��UI�I�u�W�F�N�g
    [SerializeField] private GameObject gamepadUI; // �Q�[���R���g���[���[�p��UI�I�u�W�F�N�g
    [SerializeField] private GameObject touchUI; // ���o�C���p��UI�I�u�W�F�N�g
    // public GameObject uiPrompt;
    private InputDevice lastActiveDevice; // �O��A�N�e�B�u�ɂȂ����f�o�C�X
    private bool _isDisplay = false; // UI��\�����邩�ǂ����̃t���O

    public Transform player; // �v���C���[�I�u�W�F�N�g�ւ̎Q��
    public float displayDistance = 5.0f; // UI��\�����鋗��

    void Awake()
    {

        // �v���C���[��������Ȃ��ꍇ�A�ēx��������
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    private void OnEnable()
    {
        DeviceTypeDetector.OnDeviceTypeChanged += UpdateUI;
    }

    private void OnDisable()
    {
        DeviceTypeDetector.OnDeviceTypeChanged -= UpdateUI;
    }

    private void Update()
    {

        // �v���C���[��UI�I�u�W�F�N�g�̋������v�Z
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // �����Ɋ�Â���UI��\���܂��͔�\��
        if (distance <= displayDistance)
        {
            _isDisplay = true;
        }
        else
        {
            _isDisplay = false;
        }

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
    }


    private void UpdateUI(InputDevice device)
    {
        // �f�o�C�X�^�C�v�Ɋ�Â���UI�������ݒ�
        keyboardMouseUI.SetActive(device is Keyboard || device is Mouse);
        gamepadUI.SetActive(device is Gamepad);
        touchUI.SetActive(device is Touchscreen);
    }

    void OnTriggerEnter(Collider other)
    {
        // �v���C���[���߂Â�����
        if (other.tag == "Player")
        {
            // UI��\��
            // _isDisplay = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // �v���C���[�����ꂽ��
        if (other.tag == "Player")
        {
            // UI���\��
            // _isDisplay = false;
        }
    }
}
