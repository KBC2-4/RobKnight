using System.Collections.Generic;
using UnityEngine;

public class MiniMapIconTracker : MonoBehaviour
{
    public Camera minimapCamera; // ミニマップのカメラ
    public RectTransform minimapRect; // ミニマップUIのRectTransform
    public GameObject enemyIconPrefab; // エネミーアイコンのプレハブ
    public GameObject interactiveIconPrefab; // インタラクティブアイコンのプレハブ

    private List<GameObject> enemyIcons = new List<GameObject>(); // アクティブなエネミーアイコンのリスト
    private List<GameObject> interactiveIcons = new List<GameObject>(); // アクティブなインタラクティブアイコンのリスト

    void Update()
    {
        // UpdateIcons(GameObject.FindGameObjectsWithTag("Enemy"), enemyIcons, enemyIconPrefab);
        UpdateIcons(GameObject.FindGameObjectsWithTag("Interactable"), interactiveIcons, interactiveIconPrefab);
    }

    void UpdateIcons(GameObject[] objects, List<GameObject> icons, GameObject prefab)
    {
        // アイコンをオブジェクトの数に合わせて生成
        while (icons.Count < objects.Length)
        {
            // 新しいアイコンを生成
            GameObject icon = Instantiate(prefab, minimapRect);
            icon.SetActive(true);
            icons.Add(icon);
            
            // 生成し、iconsに追加
            icons.Add(Instantiate(prefab, minimapRect));
        }
        
        // アイコンをオブジェクトの数に合わせて削除
        while (icons.Count > objects.Length)
        {
            Destroy(icons[icons.Count - 1]);
            icons.RemoveAt(icons.Count - 1);
        }

        // アイコンの位置を更新
        for (int i = 0; i < objects.Length; i++)
        {
            // ミニマップカメラのビューポート座標に変換
            Vector3 minimapPosition = minimapCamera.WorldToViewportPoint(objects[i].transform.position);
            // アイコンがミニマップの範囲内にあるか確認
            if (minimapPosition.x >= 0 && minimapPosition.x <= 1 && minimapPosition.y >= 0 && minimapPosition.y <= 1)
            {
                // ビューポート座標をUIのアンカー座標に変換
                minimapPosition.x = (minimapPosition.x * minimapRect.sizeDelta.x) - (minimapRect.sizeDelta.x * 0.5f);
                minimapPosition.y = (minimapPosition.y * minimapRect.sizeDelta.y) - (minimapRect.sizeDelta.y * 0.5f);
                icons[i].GetComponent<RectTransform>().anchoredPosition = minimapPosition;
                icons[i].SetActive(true);
            }
            else
            {
                // マップの範囲外の場合はアイコンを非表示にする
                icons[i].SetActive(false);
            }
        }
    }
}