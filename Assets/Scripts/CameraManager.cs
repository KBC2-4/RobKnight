using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject _currentActiveCamera; // ���݃A�N�e�B�u�ȃJ����
    private GameObject _mainCamera; // ���C���J����
    private GameObject _miniMapCamera; // �~�j�}�b�v�J����

    public static CameraManager Instance;   // �C���X�^���X

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // �����J������ݒ�i���C���J�����j
        _currentActiveCamera = GameObject.FindWithTag("MainCamera");
        // �����J������ݒ�i�~�j�}�b�v�J�����j
        _miniMapCamera = GameObject.FindWithTag("MiniMapCamera");        
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

        // ���ׂẴJ�����𖳌���(�~�j�}�b�v�J�����͗�O)
        foreach (var cam in cameras)
        {
            if (!cam.gameObject.CompareTag("MiniMapCamera"))
            {
                cam.gameObject.SetActive(false);
            }
        }

        // ���C���J�������A�N�e�B�u��
        _mainCamera.gameObject.SetActive(true);
    }
}