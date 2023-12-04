using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject _currentActiveCamera; // ���݃A�N�e�B�u�ȃJ����
    private GameObject _mainCamera; // ���C���J����

    public static CameraManager Instance;   // �C���X�^���X

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // �����J������ݒ�i���C���J�����j
        _currentActiveCamera = GameObject.FindWithTag("MainCamera");
        // ���C���J�������i�[
        _mainCamera = _currentActiveCamera;
    }

    // �J������؂�ւ���֐�
    public void SwitchCamera(GameObject newCamera)
    {
        if (_currentActiveCamera != null)
        {
            // ���݂̃J�������A�N�e�B�u�ɂ���
            _currentActiveCamera.SetActive(false);
        }

        // �V�����J�������A�N�e�B�u�ɂ��A���݂̃J�����Ƃ��Đݒ�
        newCamera.SetActive(true);
        _currentActiveCamera = newCamera;
    }

    // ���݃A�N�e�B�u�ȃJ���������ׂĖ��������A���C���J�����ɐ؂�ւ���
    public void SwitchToMainCamera()
    {
        // ���ׂẴJ����������
        Camera[] cameras = FindObjectsOfType<Camera>();

        // ���ׂẴJ�����𖳌���
        foreach (var cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }

        // ���C���J�������A�N�e�B�u��
        _mainCamera.gameObject.SetActive(true);
    }
}