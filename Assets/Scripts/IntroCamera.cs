using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCamera : MonoBehaviour
{
    [SerializeField] GameObject _introCamera; // イントロカメラオブジェクト
    [SerializeField] Transform _spawnPoint; // プレイヤーのスポーン位置
    private GameObject _mainCamera; // メインカメラ
    private GameObject _player; // プレイヤーオブジェクト
    private Animator _introAnimator;    // イントロカメラのアニメーター

    public event Action OnIntroAnimationComplete;   //  イントロアニメーションが終了したときに発行されるイベント

    void Start()
    {
        // タグを使って各オブジェクトを自動取得
        _player = GameObject.FindWithTag("Player");
        _mainCamera = GameObject.FindWithTag("MainCamera");

        // ゲーム開始時にイントロカメラに切り替える
        CameraManager.Instance.SwitchCamera(_introCamera);

        // イントロカメラのアニメーターを取得
        _introAnimator = _introCamera.GetComponent<Animator>();

        // プレイヤーを非表示
        // _player.SetActive(false);

        // StartCoroutine(ShowStageIntro());
    }

        void Update()
    {
        // アニメーションが終了したかどうかをチェック
        if (_introAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            OnIntroAnimationComplete?.Invoke();
            CameraManager.Instance.SwitchToMainCamera();
        }


        // デバッグ用
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            _introAnimator.speed++;
        }
#endif
    }

    IEnumerator ShowStageIntro()
    {
        // イントロシーンを表示するために数秒待つ
        yield return new WaitForSeconds(5);

        // メインカメラに切り替える
        CameraManager.Instance.SwitchCamera(_mainCamera);


        // プレイヤーを表示
        // _player.SetActive(true);
        // プレイヤーをスポーン位置に配置する
        _player.transform.position = _spawnPoint.position;
    }

    // アニメーションイベントから呼び出されるメソッド
    public void OnIntroAnimationEnd()
    {
        // メインカメラに切り替える
        CameraManager.Instance.SwitchCamera(_mainCamera);

        // プレイヤーを表示し、スポーン位置に配置
        // _player.SetActive(true);
        _player.transform.position = _spawnPoint.position;
    }
}
