using UnityEngine;

public class TitleLogoManager : MonoBehaviour
{
    public static TitleLogoManager Instance;

    public Animator titleLogoAnimator; // �^�C�g�����S��Animator
    private bool hasAnimatedOnce = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // ���̃I�u�W�F�N�g���V�[���J�ڂŔj�����Ȃ��悤�ɐݒ�
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �d������C���X�^���X�͔j��
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!hasAnimatedOnce)
        {
            titleLogoAnimator.Play("TitleLogoAnimation");
            hasAnimatedOnce = true;
        }
        else
        {
            // �A�j���[�V�������Ȃ��A���S�̌Œ�ʒu�������X�e�[�g
            titleLogoAnimator.Play("TitleLogoStatic");
        }
    }
}