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
    private float _timeRemainingF = 15f; // 推移までの時間(秒)
    [SerializeField] GameObject _canvas; // ゲームクリアCanvas
    [SerializeField] GameObject _camera; // ボス用のカメラ
    private CameraController _cameraController; // カメラにアタッチされているコントローラースクリプト
    private bool hasExecuted = false;   // 1回のみ実行されるフラグ
    [SerializeField] private PlayerController _playerController;

    void Awake()
    {

    }

    void Start()
    {
        _canvas.SetActive(false);
        _camera.SetActive(false);
        _cameraController = _camera.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_boss.isDeath)
        {
            if (!hasExecuted)
            {
                // プレイヤー操作を無効化する
                _playerController.SetInputAction(false);

                // プレイヤーを無敵、不動モードにする
                // ??????????????????????????

                _camera.SetActive(true);

                // スローモーションにする
                Time.timeScale = 0.5f;

                // カメラをボスを中心に回転させる
                _cameraController.StartRotation();

                // ゲームクリア画面を表示
                _canvas.SetActive(true);

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