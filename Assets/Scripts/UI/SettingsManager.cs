using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{

    public TMP_Dropdown windowModeDropdown;
    public Slider seVolumeSlider;
    public Slider bgmVolumeSlider;
    public AudioSource seAudioSource;
    public AudioSource bgmAudioSource;
    public AudioMixer mixer;

    // public Slider cameraRotationSpeedSlider;
    // public PlayerController playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        windowModeDropdown.value = PlayerPrefs.GetInt("WindowMode", 0);

        seVolumeSlider.value = PlayerPrefs.GetFloat("SEVolume", 1.0f);
        bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        AdjustSEVolume(seVolumeSlider.value);
        AdjustBGMVolume(bgmVolumeSlider.value);

        // cameraRotationSpeedSlider.value = PlayerPrefs.GetFloat("CameraRotationSpeed", 1.0f);
        // AdjustCameraRotationSpeed(cameraRotationSpeedSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSettings(out int width, out int height, out int windowMode, out float seVolume, out float bgmVolume/*, out float cameraRotationSpeed*/)
    {
        // �𑜓x�̓ǂݍ���
        width = PlayerPrefs.GetInt("ResolutionWidth", 1920); // �f�t�H���g�l��1920�Ƃ���
        height = PlayerPrefs.GetInt("ResolutionHeight", 1080); // �f�t�H���g�l��1080�Ƃ���

        // �E�B���h�E���[�h�̓ǂݍ���
        windowMode = PlayerPrefs.GetInt("WindowMode", 0); // �f�t�H���g�l��0 (�E�B���h�E���[�h)�Ƃ���

        // SE�EBGM���ʂ̓ǂݍ���
        seVolume = PlayerPrefs.GetFloat("SEVolume", 1.0f); // �f�t�H���g�l��1.0�Ƃ���
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f); // �f�t�H���g�l��1.0�Ƃ���

        // �J�����̉�]���x�̓ǂݍ���
        // cameraRotationSpeed = PlayerPrefs.GetFloat("CameraRotationSpeed", 1.0f); // �f�t�H���g�l��1.0�Ƃ���
    }

    public void SaveSettings(int width, int height, int windowMode, float seVolume, float bgmVolume, float cameraRotationSpeed)
    {
        // �𑜓x�̕ۑ�
        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);

        // �E�B���h�E���[�h�̕ۑ� (0: �E�B���h�E, 1: �^���t���X�N���[��, 2: �t���X�N���[��)
        PlayerPrefs.SetInt("WindowMode", windowMode);

        // SE�EBGM���ʂ̕ۑ�
        PlayerPrefs.SetFloat("SEVolume", seVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);

        // �J�����̉�]���x�̕ۑ�
        PlayerPrefs.SetFloat("CameraRotationSpeed", cameraRotationSpeed);

        // ���ۂɃf�B�X�N�ɏ�������
        PlayerPrefs.Save();
    }

    public void SetWindowMode(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 1: // FullScreen Windowed (Borderless)
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: // Exclusive FullScreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }
        PlayerPrefs.SetInt("WindowMode", modeIndex);
    }

    public void AdjustSEVolume(float volume)
    {
        seAudioSource.volume = volume;
        PlayerPrefs.SetFloat("SEVolume", volume);
        mixer.SetFloat("SEVolume", volume);
    }

    public void AdjustBGMVolume(float volume)
    {
        bgmAudioSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    /*
    <summary>
        �N�I���e�B(�i��)��ύX����   
    </summary>
    <param name="qualityIndex">�N�I���e�B�̃C���f�b�N�X</param>
    */
        public void ChangeQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex, true);
        // �m�F�̂��߂Ƀ��O���o��
        Debug.Log("�N�I���e�B��" + QualitySettings.names[qualityIndex] + "�ɕύX���܂���");
    }

    // public void AdjustCameraRotationSpeed(float speed)
    // {
    //     if (SceneManager.GetActiveScene().name != "GameMainScene")
    //     {
    //         // �Q�[�����C���V�[���ł͂Ȃ��ꍇ�A���\�b�h�̏������X�L�b�v
    //         return;
    //     }

    //     // playerMovement���ݒ肳��Ă��Ȃ��ꍇ
    //     if (playerMovement == null)
    //     {
    //         // ���\�b�h�̏������X�L�b�v
    //         return;
    //     }

    //     playerMovement.sensitivity = speed;
    //     PlayerPrefs.SetFloat("CameraRotationSpeed", speed);
    // }
}
