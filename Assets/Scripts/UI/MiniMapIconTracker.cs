using UnityEngine;

public class MiniMapIconTracker : MonoBehaviour
{
    public Transform target; // ゲームワールドの対象
    public RectTransform minimapRectTransform; // ミニマップのRectTransform
    public Vector2 worldSize = new Vector2(1000, 1000); // ワールドのサイズ
    public RectTransform icon; // ミニマップ上のアイコン

    void Update()
    {
        // Vector2 minimapSize = new Vector2(minimapRectTransform.rect.width, minimapRectTransform.rect.height);
        // Vector2 minimapPosition = WorldToMinimapPosition(target.position, minimapSize);
        // minimapRectTransform.anchoredPosition = minimapPosition;
        // icon.anchoredPosition = minimapPosition;
    }

    Vector2 WorldToMinimapPosition(Vector3 worldPosition, Vector2 minimapSize)
    {
        Vector2 normalizedPosition = new Vector2(
            (worldPosition.x + worldSize.x / 2) / worldSize.x,
            (worldPosition.z + worldSize.y / 2) / worldSize.y
        );
        return new Vector2(
            (normalizedPosition.x - 0.5f) * minimapSize.x,
            (normalizedPosition.y - 0.5f) * minimapSize.y
        );
    }
}