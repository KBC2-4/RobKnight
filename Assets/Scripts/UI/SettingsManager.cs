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

    [SerializeField, Header("ウィンドウモード設定用のドロップダウン")] TMP_Dropdown _windowModeDropdown;
    [SerializeField, Header("品質設定用のドロップダウン")] TMP_Dropdown _qualityDropdown; // 品質設定用のドロップダウン
    [SerializeField, Header("SE用のスライドバー")] Slider _seVolumeSlider;
    [SerializeField, Header("BGM用のスライドバー")] Slider _bgmVolumeSlider;
    [SerializeField, Header("SE用のオーディオソース")] AudioSource _seAudioSource;
    [SerializeField, Header("BGM用のオーディオソース")] AudioSource _bgmAudioSource;
    [SerializeField, Header("オーディオミキサー")] AudioMixer _mixer;
    
    // public Slider cameraRotationSpeedSlider;
    // public PlayerController playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        _windowModeDropdown.value = PlayerPrefs.GetInt("WindowMode", 0);

        _seVolumeSlider.value = PlayerPrefs.GetFloat("SEVolume", 1.0f);
        _bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        AdjustSEVolume(_seVolumeSlider.value);
        AdjustBGMVolume(_bgmVolumeSlider.value);
        _qualityDropdown.value = QualitySettings.GetQualityLevel();
        Debug.Log("" + QualitySettings.GetQualityLevel());

        // cameraRotationSpeedSlider.value = PlayerPrefs.GetFloat("CameraRotationSpeed", 1.0f);
        // AdjustCameraRotationSpeed(cameraRotationSpeedSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSettings(out int width, out int height, out int windowMode, out float seVolume, out float bgmVolume/*, out float cameraRotationSpeed*/)
    {
        // 解像度の読み込み
        width = PlayerPrefs.GetInt("ResolutionWidth", 1920); // デフォルト値を1920とする
        height = PlayerPrefs.GetInt("ResolutionHeight", 1080); // デフォルト値を1080とする

        // ウィンドウモードの読み込み
        windowMode = PlayerPrefs.GetInt("WindowMode", 0); // デフォルト値を0 (ウィンドウモード)とする



        // SE・BGM音量の読み込み
        seVolume = PlayerPrefs.GetFloat("SEVolume", 1.0f); // デフォルト値を1.0とする
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f); // デフォルト値を1.0とする
        // あなたは
        // カメラの回転速度の読み込み
        // cameraRotationSpeed = PlayerPrefs.GetFloat("CameraRotationSpeed", 1.0f); // デフォルト値を1.0とする
    }

    public void SaveSettings(int width, int height, int windowMode, float seVolume, float bgmVolume, float cameraRotationSpeed)
    {
        // 解像度の保存
        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);

        // ウィンドウモードの保存 (0: ウィンドウ, 1: 疑似フルスクリーン, 2: フルスクリーン)
        PlayerPrefs.SetInt("WindowMode", windowMode);

        // SE・BGM音量の保存
        PlayerPrefs.SetFloat("SEVolume", seVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);

        // カメラの回転速度の保存
        PlayerPrefs.SetFloat("CameraRotationSpeed", cameraRotationSpeed);

        // 実際にディスクに書き込む
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
        _seAudioSource.volume = volume;
        PlayerPrefs.SetFloat("SEVolume", volume);
        _mixer.SetFloat("SEVolume", volume);
    }

    public void AdjustBGMVolume(float volume)
    {
        _bgmAudioSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    /*
    <summary>
        クオリティ(品質)を変更する
    </summary>
    <param name="qualityIndex">クオリティのインデックス</param>
    */
        public void ChangeQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex, true);
        // 確認のためにログを出力
        Debug.Log("クオリティを" + QualitySettings.names[qualityIndex] + "に変更しました");
    }

    // public void AdjustCameraRotationSpeed(float speed)
    // {
    //     if (SceneManager.GetActiveScene().name != "GameMainScene")
    //     {
    //         // ゲームメインシーンではない場合、メソッドの処理をスキップ
    //         return;
    //     }

    //     // playerMovementが設定されていない場合
    //     if (playerMovement == null)
    //     {
    //         // メソッドの処理をスキップ
    //         return;
    //     }

    //     playerMovement.sensitivity = speed;
    //     PlayerPrefs.SetFloat("CameraRotationSpeed", speed);
    // }
}
