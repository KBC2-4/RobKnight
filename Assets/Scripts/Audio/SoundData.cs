using UnityEngine;

[System.Serializable]
public class SoundData
{

    [SerializeField] private string soundType; //�����̎��
    //soundType��Getter
    public string SoundType => soundType;
    [SerializeField] private AudioClip audioClip;   //�����f�[�^
    //audioClip��Getter
    public AudioClip AudioClip => audioClip;
    [Range(0, 1)]
    [SerializeField] private float volume = 1.0f;   //����
}
