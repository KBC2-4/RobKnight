using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
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


    // Start is called before the first frame update
    void Start()
    {
        //カメラの位置を設定
        //Vector3 position = target.transform.position;
        //position.y += 10;
        //gameObject.transform.position = position;
        
        //ゲーム開始時にカメラとターゲットの距離を取得
        offset = gameObject.transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットの位置にカメラを追従させる
        gameObject.transform.position = target.transform.position + offset;
    }
}
