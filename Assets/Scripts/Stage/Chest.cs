using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Chest : MonoBehaviour
{
    private Animator animator;
    public GameObject itemPrefab; // アイテムのプレハブ
    public Text messageText;      // UIテキスト

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
        StartCoroutine(SpawnItem());
        DisplayMessage("宝箱が開かれました！");
        isOpen = true; // 宝箱が開かれたことをフラグで示す
    }


    // アイテムを生成するコルーチン
    private IEnumerator SpawnItem()
    {
        // ランダムな位置にアイテムを生成
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 2f, Random.Range(-1f, 1f));
        GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);


        // 3秒後にアイテムを削除（適宜調整）
        yield return new WaitForSeconds(3f);

        Destroy(newItem);
    }

    // メッセージを表示するメソッド
    private void DisplayMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
    }
}
 





