using UnityEngine;

public class MiniMapIconTracker : MonoBehaviour
{
    public Transform target; // �Q�[�����[���h�̑Ώ�
    public RectTransform minimapRectTransform; // �~�j�}�b�v��RectTransform
    public Vector2 worldSize = new Vector2(1000, 1000); // ���[���h�̃T�C�Y
    public RectTransform icon; // �~�j�}�b�v��̃A�C�R��

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