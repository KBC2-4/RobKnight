using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    private bool isOpen = false;   // 宝箱が開かれたかどうかのフラグ

    private void OnTriggerEnter(Collider other)
    {
        // 他のオブジェクトがトリガーコライダーに接触した時の処理
        if (other.CompareTag("Player") && !isOpen)
        {
            OpenBox();
        }
    }

    // 宝箱が開かれたときに呼ばれるメソッド
    private void OpenBox()
    {
        isOpen = true; // 宝箱が開かれたことをフラグで示す
    }

}
