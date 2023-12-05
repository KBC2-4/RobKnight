using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _bgmAudioSource;   //BGM AudioSource
    [SerializeField] private AudioSource _seAudioSource;    //SE AudioSource

    [SerializeField]private List<SoundData> _bgmSoundData;   //BGM�̃��X�g
    [SerializeField]private List<SoundData> _seSoundData;   //SE�̃��X�g

    [SerializeField]private float _masterVolume =1.0f;
    [SerializeField]private float _bgmMasterVolume =1.0f;
    [SerializeField]private float _seMasterVolume =1.0f;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            //�V�����V�[����ǂݍ���ł��j�󂹂�Ȃ��悤�ɂ���
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// SE�Đ��֐�
    /// </summary>
    /// <param name="soundType">�Đ�����SE�̎��</param>
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