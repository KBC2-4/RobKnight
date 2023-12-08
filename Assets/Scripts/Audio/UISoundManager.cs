using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISoundManager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public static UISoundManager Instance { get; private set; }

    [SerializeField] AudioSource audioSource; // �I�[�f�B�I�\�[�X
    [SerializeField] AudioClip hoverSound;    // �J�[�\���ړ���
    [SerializeField] AudioClip clickSound;    // ���艹

    private bool _isProgrammaticSelect = false;  // SE�̍Đ��𖳌�������t���O

    // <sumary>
    // SE�̍Đ��𖳌�������
    // </summary>
    public void SetProgrammaticSelect()
    {
        _isProgrammaticSelect = true;
    }

    public bool isProgrammaticSelect
    {
        get { return _isProgrammaticSelect; }
        set { _isProgrammaticSelect = value; }
    }

    void Awake()
    {
        // GameObject�����[�g�Ɉړ�
        // transform.parent = null;

        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // �V�[����EventSystem�ōŏ��ɑI�����Ă���{�^����ݒ肵�Ă���ꍇ��SE���Đ�����Ȃ��悤��true�ɂ���
        _isProgrammaticSelect = true;
    }

    // �J�[�\�����v�f�ɓ������Ƃ��ɌĂ΂��
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySound(hoverSound);
    }

    // �v�f���N���b�N���ꂽ�Ƃ��ɌĂ΂��
    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickSound);
    }

    // �T�E���h���Đ����郁�\�b�h
    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}