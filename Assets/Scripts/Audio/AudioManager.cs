using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //[SerializeField] private AudioSource _bgmAudioSource;   //BGM AudioSource
    //[SerializeField] private AudioSource _seAudioSource;    //SE AudioSource
    [SerializeField] private AudioSource _audioSource;    //SE AudioSource

    //[SerializeField]private List<SoundData> _bgmSoundData;   //BGMのリスト
    //[SerializeField]private List<SoundData> _seSoundData;   //SEのリスト
    [SerializeField]private List<SoundData> _soundData;   //SEのリスト
    private Dictionary<string, AudioClip> _seDictionary; //SEの辞書

    [SerializeField]private float _masterVolume =1.0f;
    [SerializeField]private float _bgmMasterVolume =1.0f;
    [SerializeField]private float _seMasterVolume =1.0f;
    
    [SerializeField] AudioMixer audioMixer;
    private AudioMixerGroup seMixerGroup;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            //新しいシーンを読み込んでも破壊せれないようにする
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        // 辞書の初期化
        _seDictionary = new Dictionary<string, AudioClip>();

        // ここで、_seSoundDataからデータを辞書に追加します
        foreach (var soundData in _soundData) {
            _seDictionary[soundData.FileName] = soundData.AudioClip;
        }
    }
    
    private void Start()
    {
        seMixerGroup = audioMixer.FindMatchingGroups("SE")[0];
    }

    /// <summary>
    /// SE再生関数
    /// </summary>
    /// <param name="fileName">再生するSEの種類</param>
    public void PlaySE(string fileName)
    {
        if (_seDictionary.TryGetValue(fileName, out var clip))
        {
            _audioSource.outputAudioMixerGroup = seMixerGroup; // オーディオミキサーグループを適用
            //SE個々のボリュームに適応
            _audioSource.volume = _soundData.Find(data => data.FileName == fileName).Volume;
            _audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError(fileName + "が見つかりませんでした");
        }
        
        // SoundData data = _seSoundData.Find(data => data.SoundType == soundType);
        // _seAudioSource.outputAudioMixerGroup = seMixerGroup; // オーディオミキサーグループを適用
        // _seAudioSource.PlayOneShot(data.AudioClip);
    }

    //public void PlayAudio()
    //{
    //    _bgmAudioSource.Play();
    //}

    //public void StopAudio()
    //{
    //    _bgmAudioSource.Stop();
    //}

    //public void PauseAudio()
    //{
    //    _bgmAudioSource.Pause();
    //}
}