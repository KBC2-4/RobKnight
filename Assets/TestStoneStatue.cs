using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TestStoneStatue : MonoBehaviour
{
    /*
     * 各変数の宣言の仕方
    [Serializable] Int32 a = 3;　　private publicの中間
    private Int32 _b = 4;
    public Int32 c = 5;
    */

    public bool attacked = false; //攻撃を食らったのか
    public bool during_rotation = false; //回転中なのか

    private float _old_rotaey = 0;


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

        if (attacked == true && during_rotation == false)
        {
            _old_rotaey = transform.localEulerAngles.y;
            during_rotation = true;
            attacked = false;
        }

        if (during_rotation)
        {
            if (_old_rotaey + 15 >= transform.localEulerAngles.y)
            {
                transform.Rotate(0f, 0f, 6 * Time.deltaTime); //回転
            }
            else
            {
                during_rotation = false;
            }

            //UnityEngine.Debug.Log(transform.localEulerAngles.y); //デバッグ用
            //UnityEngine.Debug.Log(_old_rotaey + 15); //デバッグ用
        }
    }
}