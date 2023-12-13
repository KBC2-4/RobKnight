using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameClearController : MonoBehaviour
{
    [SerializeField] private EnemyController _boss;
    [SerializeField] private TMP_Text _countdownText; // UIテキストへの参照
    private static float _timeRemainingF = 30f;
    [SerializeField] private GameObject _canvas; // ゲームクリアCanvas
    [SerializeField] private GameObject _camera; // ボス用のカメラ
    [SerializeField] private GameObject _cameraParent; // ボス用のカメラの親オブジェクト
    private CameraController _cameraController; // カメラにアタッチされているコントローラースクリプト
    [SerializeField] private GameObject _cameraAnimatorController; // ボス用のカメラの親オブジェクトを管理しているコントローラー
    private CameraAnimatorController _cameraAnimatorControllerS; // カメラの親オブジェクトにアタッチされているコントローラースクリプト
    private bool hasExecuted = false;   // 1回のみ実行されるフラグ
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private AudioSource _bgmAudioSource; // BGM再生に使用するオーディオソース
    [SerializeField] private AudioClip _bgmClip; // 再生するBGM

    void Awake()
    {
        if(_playerController == null)
        {
            // _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            _playerController = FindObjectOfType<PlayerController>();
        }

        if (_cameraAnimatorController != null)
        {
            _cameraAnimatorControllerS = _cameraAnimatorController.GetComponent<CameraAnimatorController>();
        }
        
    }

    private void OnEnable()
    {
        if (_cameraAnimatorController != null)
        {
            _cameraAnimatorControllerS.OnAnimationComplete += HandleAnimationComplete;
        }
    }

    private void OnDisable()
    {

        if (_cameraAnimatorController != null)
        {
            _cameraAnimatorControllerS.OnAnimationComplete -= HandleAnimationComplete;
        }
    }

    /// <summary>
    /// アニメーションが完了した際に呼び出されるハンドラー。この関数ではUIを表示状態に切り替えます。
    /// アニメーションの完了を検知して、UI表示フラグをtrueに設定し、ガイドキャンバスをアクティブにします。
    /// </summary>
    private void HandleAnimationComplete()
    {
        // 通常の速度に戻す
        Time.timeScale = 1f;

        // 新しいクリップを設定
        _bgmAudioSource.clip = _bgmClip;
        // 新しいクリップを再生開始
        _bgmAudioSource.Play();

        // アニメーション完了後にUIを表示
        // ゲームクリア画面を表示
        _canvas.SetActive(true);
    }

    void Start()
    {
        _canvas.SetActive(false);
        // _camera.SetActive(false);

        if(_cameraController != null)
        {
            _cameraController = _camera.GetComponent<CameraController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_boss.isDeath)
        {
            if (!hasExecuted)
            {
                
                if (_playerController != null)
                {
                    // プレイヤー操作を無効化する
                    // _playerController.SetInputAction(false);
                    // プレイヤーを無敵にする
                    _playerController.IsInfinity = true;
                }

                // ガイドバーを非表示にする
                GuideBarController.Instance.SetUIVisibility(false);

                _camera.SetActive(true);

                // スローモーションにする
                Time.timeScale = 0.3f;

                // カメラをボスを中心に回転させる
                // _cameraController.StartRotation();
                _cameraAnimatorControllerS.StartRotation();

                //// ゲームクリア画面を表示
                //_canvas.SetActive(true);

                // フラグを更新
                hasExecuted = true;
            }

            // 数秒後に画面遷移（タイトルへ推移）
            if (_timeRemainingF > 1)
            {
                // 経過時間をカウント
                _timeRemainingF -= Time.deltaTime;
                // _countdownText.text = Mathf.Round(_timeRemainingF).ToString();
                _countdownText.text = _timeRemainingF.ToString("F0") + "秒後にタイトルへ戻ります";

            }
            else
            {

                _countdownText.text = "タイトルへ戻ります...";
                _camera.SetActive(false);
                SceneManager.LoadScene("Title");
            }
        }
    }

    void OnDestroy()
    {
        hasExecuted = false;


    }
}