using System.Collections.Generic;
using UnityEngine;

public class MiniMapIconTracker : MonoBehaviour
{
    public Camera minimapCamera; // �~�j�}�b�v�̃J����
    public RectTransform minimapRect; // �~�j�}�b�vUI��RectTransform
    public GameObject enemyIconPrefab; // �G�l�~�[�A�C�R���̃v���n�u
    public GameObject interactiveIconPrefab; // �C���^���N�e�B�u�A�C�R���̃v���n�u

    private List<GameObject> enemyIcons = new List<GameObject>(); // �A�N�e�B�u�ȃG�l�~�[�A�C�R���̃��X�g
    private List<GameObject> interactiveIcons = new List<GameObject>(); // �A�N�e�B�u�ȃC���^���N�e�B�u�A�C�R���̃��X�g

    void Update()
    {
        // UpdateIcons(GameObject.FindGameObjectsWithTag("Enemy"), enemyIcons, enemyIconPrefab);
        UpdateIcons(GameObject.FindGameObjectsWithTag("Interactable"), interactiveIcons, interactiveIconPrefab);
    }

    void UpdateIcons(GameObject[] objects, List<GameObject> icons, GameObject prefab)
    {
        // �A�C�R�����I�u�W�F�N�g�̐��ɍ��킹�Đ���
        while (icons.Count < objects.Length)
        {
            // �V�����A�C�R���𐶐�
            GameObject icon = Instantiate(prefab, minimapRect);
            icon.SetActive(true);
            icons.Add(icon);
            
            // �������Aicons�ɒǉ�
            icons.Add(Instantiate(prefab, minimapRect));
        }
        
        // �A�C�R�����I�u�W�F�N�g�̐��ɍ��킹�č폜
        while (icons.Count > objects.Length)
        {
            Destroy(icons[icons.Count - 1]);
            icons.RemoveAt(icons.Count - 1);
        }

        // �A�C�R���̈ʒu���X�V
        for (int i = 0; i < objects.Length; i++)
        {
            // �~�j�}�b�v�J�����̃r���[�|�[�g���W�ɕϊ�
            Vector3 minimapPosition = minimapCamera.WorldToViewportPoint(objects[i].transform.position);
            // �A�C�R�����~�j�}�b�v�͈͓̔��ɂ��邩�m�F
            if (minimapPosition.x >= 0 && minimapPosition.x <= 1 && minimapPosition.y >= 0 && minimapPosition.y <= 1)
            {
                // �r���[�|�[�g���W��UI�̃A���J�[���W�ɕϊ�
                minimapPosition.x = (minimapPosition.x * minimapRect.sizeDelta.x) - (minimapRect.sizeDelta.x * 0.5f);
                minimapPosition.y = (minimapPosition.y * minimapRect.sizeDelta.y) - (minimapRect.sizeDelta.y * 0.5f);
                icons[i].GetComponent<RectTransform>().anchoredPosition = minimapPosition;
                icons[i].SetActive(true);
            }
            else
            {
                // �}�b�v�͈̔͊O�̏ꍇ�̓A�C�R�����\���ɂ���
                icons[i].SetActive(false);
            }
        }
    }
}