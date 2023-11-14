using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStoneStatue : MonoBehaviour
{


   void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform myTransform = this.transform;

        //永遠と回転する処理
        // transformを取得
        //// ワールド座標基準で、現在の回転量へ加算する
        //myTransform.Rotate(0, 1.0f, 0, Space.World);


        Vector3 localAngle = myTransform.localEulerAngles;
        localAngle.z = 15.0f; // ローカル座標を基準に、z軸を軸にした回転を10度に変更

        myTransform.localEulerAngles = localAngle; // 回転角度を設定

    }
}
