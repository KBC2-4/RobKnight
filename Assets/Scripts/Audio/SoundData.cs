using UnityEngine;

[System.Serializable]
public class SoundData
{

    // [SerializeField] private string soundType; //�����̎��
    //soundType��Getter
    // public string SoundType => soundType;
    
    [SerializeField] private string fileName; // �����t�@�C���̖��O
    public string FileName => fileName;
    
    [SerializeField] private AudioClip audioClip;   //�����f�[�^
    //audioClip��Getter
    public AudioClip AudioClip => audioClip;
    [SerializeField, Range(0, 1)] private float volume = 1.0f;   //����
    public float Volume => volume;
}
