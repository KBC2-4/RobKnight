using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //[SerializeField] private AudioSource _bgmAudioSource;   //BGM AudioSource
    //[SerializeField] private AudioSource _seAudioSource;    //SE AudioSource
    [SerializeField] private AudioSource _audioSource;    //SE AudioSource

    //[SerializeField]private List<SoundData> _bgmSoundData;   //BGM�̃��X�g
    //[SerializeField]private List<SoundData> _seSoundData;   //SE�̃��X�g
    [SerializeField]private List<SoundData> _soundData;   //SE�̃��X�g
    private Dictionary<string, AudioClip> _seDictionary; //SE�̎���

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
            //�V�����V�[����ǂݍ���ł��j�󂹂�Ȃ��悤�ɂ���
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        // �����̏�����
        _seDictionary = new Dictionary<string, AudioClip>();

        // �����ŁA_seSoundData����f�[�^�������ɒǉ����܂�
        foreach (var soundData in _soundData) {
            _seDictionary[soundData.FileName] = soundData.AudioClip;
        }
    }
    
    private void Start()
    {
        seMixerGroup = audioMixer.FindMatchingGroups("SE")[0];
    }

    /// <summary>
    /// SE�Đ��֐�
    /// </summary>
    /// <param name="fileName">�Đ�����SE�̎��</param>
    public void PlaySE(string fileName)
    {
        if (_seDictionary.TryGetValue(fileName, out var clip))
        {
            _audioSource.outputAudioMixerGroup = seMixerGroup; // �I�[�f�B�I�~�L�T�[�O���[�v��K�p
            //SE�X�̃{�����[���ɓK��
            _audioSource.volume = _soundData.Find(data => data.FileName == fileName).Volume;
            _audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError(fileName + "��������܂���ł���");
        }
        
        // SoundData data = _seSoundData.Find(data => data.SoundType == soundType);
        // _seAudioSource.outputAudioMixerGroup = seMixerGroup; // �I�[�f�B�I�~�L�T�[�O���[�v��K�p
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