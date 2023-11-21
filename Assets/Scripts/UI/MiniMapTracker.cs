using UnityEngine;

public class MiniMapTracker : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public Transform item;
    public RectTransform miniMapPlayerIcon;
    public RectTransform miniMapEnemyIcon;
    public RectTransform miniMapItemIcon;

    void Update()
    {
        UpdateIconPosition(player, miniMapPlayerIcon);
        UpdateIconPosition(enemy, miniMapEnemyIcon);
        UpdateIconPosition(item, miniMapItemIcon);
    }

    void UpdateIconPosition(Transform worldObject, RectTransform miniMapIcon)
    {
        // if (worldObject != null && miniMapIcon != null)
        // {
        //     Vector3 worldPosition = worldObject.position;
        //     // ���[���h���W���~�j�}�b�v�̍��W�ɕϊ�
        //     Vector3 miniMapPosition = miniMapIcon.InverseTransformPoint(worldPosition);
        //     miniMapIcon.anchoredPosition = miniMapPosition;
        //     miniMapIcon.gameObject.SetActive(true);
        // }
    }
}