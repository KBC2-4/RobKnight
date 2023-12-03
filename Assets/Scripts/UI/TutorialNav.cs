using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialNav : MonoBehaviour
{
    //最初にフォーカスするゲームオブジェクト
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _previousButton;

    [SerializeField] GameObject _page1Panel;
    [SerializeField] GameObject _page2Panel;
    [SerializeField] GameObject _playerCanvas;

    private PlayerController _player;

    [SerializeField] IntroCamera _introCamera; // イントロカメラ

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
        OnPushButtonP1();
    }

    public void OnPushButtonP1()
    {
        EventSystem.current.SetSelectedGameObject(_previousButton);
        _page1Panel.SetActive(true);
        _page2Panel.SetActive(false);
    }
    public void OnPushButtonP2()
    {
        _page1Panel.SetActive(false);
        _page2Panel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_previousButton);
    }
    public void OnPushButtonFin()
    {
        _page1Panel.SetActive(false);
        _page2Panel.SetActive(false);
        _playerCanvas.SetActive(true);
        _player?.SetInputAction(true);
    }
}
