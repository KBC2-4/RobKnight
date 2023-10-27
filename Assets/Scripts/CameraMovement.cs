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
    /// プレイヤーとカメラの相対位置
    /// </summary>
    public Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        //カメラの位置を設定
        Vector3 position = target.transform.position;
        position.y += 10;
        gameObject.transform.position = position;
        
        //ゲーム開始時にカメラとターゲットの距離を取得
        offset = gameObject.transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの位置にカメラを追従させる
        gameObject.transform.position = target.transform.position + offset;

        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
        //transform.Translate(movement);
    }
}
