using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialNav : MonoBehaviour
{
    //最初にフォーカスするゲームオブジェクト
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _finButton;

    [SerializeField] GameObject _page1Panel;
    [SerializeField] GameObject _page2Panel;
    [SerializeField] GameObject _playerCanvas;

    private PlayerController _player;

    [SerializeField] IntroCamera _introCamera; // イントロカメラ
    private Animator _page1PanelAnimator; // 1ページ目のアニメーター
    private Animator _page2PanelAnimator; // 2ページ目のアニメーター
    [SerializeField] private AudioSource _seAudioSource; // ループさせない為SE再生に使用する一概念の無いオーディオソース
    [SerializeField] private AudioClip _seOpen; // UIを開くSE
    [SerializeField] private AudioClip _seClose; // UIを閉じるSE


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
        // SE再生
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
        // アニメーターの現在のステートの長さを取得
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
    
        // アニメーションの長さだけ待機
        yield return new WaitForSeconds(animationLength);

        // アニメーションが終わったらパネルを非アクティブにする
        panel.SetActive(false);
        // プレイヤーキャンバスをアクティブにする
        _playerCanvas.SetActive(true);
    }

    public void OnPushButtonFin()
    {
        if (_player != null)
        {
            // プレイヤーが入力を受け付けるようにする
            _player.SetInputAction(true);
        }
        else
        {
            Debug.LogError("Player is null!");
        }
        // SE再生
        _seAudioSource.PlayOneShot(_seClose);
        _page2PanelAnimator.SetTrigger("CloseTrigger");
        StartCoroutine(ClosePanelAfterAnimation(_page2PanelAnimator, _page2Panel));
        _page1Panel.SetActive(false);
    }
}
