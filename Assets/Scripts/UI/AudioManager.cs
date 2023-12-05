using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _bgmAudioSource;   //BGM AudioSource
    [SerializeField] private AudioSource _seAudioSource;    //SE AudioSource

    [SerializeField]private List<SoundData> _bgmSoundData;   //BGMのリスト
    [SerializeField]private List<SoundData> _seSoundData;   //SEのリスト

    [SerializeField]private float _masterVolume =1.0f;
    [SerializeField]private float _bgmMasterVolume =1.0f;
    [SerializeField]private float _seMasterVolume =1.0f;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            //新しいシーンを読み込んでも破壊せれないようにする
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// SE再生関数
    /// </summary>
    /// <param name="soundType">再生するSEの種類</param>
    public void PlaySE(string soundType)
    {
        SoundData data = _seSoundData.Find(data => data.SoundType == soundType);
        _seAudioSource.PlayOneShot(data.AudioClip);
    }

    public void PlayAudio()
    {
        _bgmAudioSource.Play();
    }

    public void StopAudio()
    {
        _bgmAudioSource.Stop();
    }

    public void PauseAudio()
    {
        _bgmAudioSource.Pause();
    }
}