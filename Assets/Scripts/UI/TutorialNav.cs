using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialNav : MonoBehaviour
{
    //�ŏ��Ƀt�H�[�J�X����Q�[���I�u�W�F�N�g
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _finButton;

    [SerializeField] GameObject _page1Panel;
    [SerializeField] GameObject _page2Panel;
    [SerializeField] GameObject _playerCanvas;

    private PlayerController _player;

    [SerializeField] IntroCamera _introCamera; // �C���g���J����
    private Animator _page1PanelAnimator; // 1�y�[�W�ڂ̃A�j���[�^�[
    private Animator _page2PanelAnimator; // 2�y�[�W�ڂ̃A�j���[�^�[
    [SerializeField] private AudioSource _seAudioSource; // ���[�v�����Ȃ���SE�Đ��Ɏg�p�����T�O�̖����I�[�f�B�I�\�[�X
    [SerializeField] private AudioClip _seOpen; // UI���J��SE
    [SerializeField] private AudioClip _seClose; // UI�����SE


    void OnEnable()
    {
        if (_introCamera != null)
        {
            _introCamera.OnIntroAnimationComplete += HandleIntroAnimationComplete;
        }
    }

    private void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            _player = playerObject.GetComponent<PlayerController>();
        }

        _page1PanelAnimator = _page1Panel.GetComponent<Animator>();
        _page2PanelAnimator = _page2Panel.GetComponent<Animator>();
    }

    void Start()
    {
        _page1Panel.SetActive(false);
        _page2Panel.SetActive(false);
        _player?.SetInputAction(false);
        _playerCanvas.SetActive(false);
        
    }

        void OnDisable()
    {
        if (_introCamera != null)
        {
            _introCamera.OnIntroAnimationComplete -= HandleIntroAnimationComplete;
        }
    }

        private void HandleIntroAnimationComplete()
    {
        // SE�Đ�
        _seAudioSource.PlayOneShot(_seOpen);
        OnPushButtonP1();
        _page1PanelAnimator.SetTrigger("OpenTrigger");
    }

    public void OnPushButtonP1()
    {
        EventSystem.current.SetSelectedGameObject(_nextButton);
        _page1Panel.SetActive(true);
        _page2Panel.SetActive(false);
    }
    public void OnPushButtonP2()
    {
        _page1Panel.SetActive(false);
        _page2Panel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_finButton);
    }
    
    private IEnumerator ClosePanelAfterAnimation(Animator animator, GameObject panel)
    {
        // �A�j���[�^�[�̌��݂̃X�e�[�g�̒������擾
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
    
        // �A�j���[�V�����̒��������ҋ@
        yield return new WaitForSeconds(animationLength);

        // �A�j���[�V�������I�������p�l�����A�N�e�B�u�ɂ���
        panel.SetActive(false);
        // �v���C���[�L�����o�X���A�N�e�B�u�ɂ���
        _playerCanvas.SetActive(true);
    }

    public void OnPushButtonFin()
    {
        if (_player != null)
        {
            // �v���C���[�����͂��󂯕t����悤�ɂ���
            _player.SetInputAction(true);
        }
        else
        {
            Debug.LogError("Player is null!");
        }
        // SE�Đ�
        _seAudioSource.PlayOneShot(_seClose);
        _page2PanelAnimator.SetTrigger("CloseTrigger");
        StartCoroutine(ClosePanelAfterAnimation(_page2PanelAnimator, _page2Panel));
        _page1Panel.SetActive(false);
    }
}
