using UnityEngine;

[System.Serializable]
public class SoundData
{

    [SerializeField] private string soundType; //音声の種類
    //soundTypeのGetter
    public string SoundType => soundType;
    [SerializeField] private AudioClip audioClip;   //音声データ
    //audioClipのGetter
    public AudioClip AudioClip => audioClip;
    [Range(0, 1)]
    [SerializeField] private float volume = 1.0f;   //音量
}
