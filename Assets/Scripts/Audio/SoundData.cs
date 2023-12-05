using UnityEngine;

[System.Serializable]
public class SoundData
{

    [SerializeField] private string soundType; //‰¹º‚ÌŽí—Þ
    //soundType‚ÌGetter
    public string SoundType => soundType;
    [SerializeField] private AudioClip audioClip;   //‰¹ºƒf[ƒ^
    //audioClip‚ÌGetter
    public AudioClip AudioClip => audioClip;
    [Range(0, 1)]
    [SerializeField] private float volume = 1.0f;   //‰¹—Ê
}
