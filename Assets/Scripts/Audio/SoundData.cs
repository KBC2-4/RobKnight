using UnityEngine;

[System.Serializable]
public class SoundData
{

    // [SerializeField] private string soundType; //音声の種類
    //soundTypeのGetter
    // public string SoundType => soundType;
    
    [SerializeField] private string fileName; // 音声ファイルの名前
    public string FileName => fileName;
    
    [SerializeField] private AudioClip audioClip;   //音声データ
    //audioClipのGetter
    public AudioClip AudioClip => audioClip;
    [SerializeField, Range(0, 1)] private float volume = 1.0f;   //音量
    public float Volume => volume;
}
