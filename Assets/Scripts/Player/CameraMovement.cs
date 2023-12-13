using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public enum State
    {
        Follow,
        Shake
    }

    /// <summary>
    /// プレイヤーのTransform
    /// </summary>
    [SerializeField]
    [Tooltip("追従させたいターゲット")]
    private GameObject target;

    /// <summary>
    /// ターゲットとカメラの相対位置
    /// </summary>
    public Vector3 offset;

    private Vector3 _centerPosition;
    private Vector3 _shakePower;

    private State _cameraState;
    public State CameraState
    {
        get => _cameraState;
        set=> _cameraState = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameraState = State.Follow;

        //カメラの位置を設定
        Vector3 position = target.transform.position;
        position.z -= 5;
        position.y += 10;
        gameObject.transform.position = position;


        //ゲーム開始時にカメラとターゲットの距離を取得
        offset = gameObject.transform.position - target.transform.position;
        _centerPosition = transform.position;
        _shakePower.x = 0.5f;
    }

    private void LateUpdate()
    {
        switch (_cameraState)
        {
            case State.Follow:
                //ターゲットの位置にカメラを追従させる
                gameObject.transform.position = target.transform.position + offset;
                break;
            case State.Shake:
                ShakeCamera();
                break;
            default:
                break;
        }
        
    }

    /// <summary>
    /// カメラのターゲットを切り替える
    /// </summary>
    /// <param name="newTarget">新たにターゲットにしたいオブジェクト</param>
    public void SetCameraTarget(GameObject newTarget)
    {
        target = newTarget;

        //カメラの位置を設定
        Vector3 position = target.transform.position;
        position.z -= 5;
        position.y += 10;
        gameObject.transform.position = position;

        //カメラとターゲットの距離を取得
        offset = gameObject.transform.position - target.transform.position;
    }

    public void ShakeCamera()
    {
        transform.position = _centerPosition + _shakePower;
        _shakePower *= -1;
    }
}
