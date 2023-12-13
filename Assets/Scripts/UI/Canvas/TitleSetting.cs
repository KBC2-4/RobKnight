using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleSetting : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    private GameObject lastSelectedButton; // �Ō�ɑI�����ꂽ�{�^��

    // Start is called before the first frame update
    void Start()
    {
        // �ݒ��ʂ��A�N�e�B�u�ɐݒ�
        _settingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // <summary>
    // �u�ݒ�v�{�^������Ăяo����� �ݒ��ʂ��J��
    // </summary>
    public void ShowSettingsPanel()
    {
        // SE���Đ�����Ȃ��悤�ɂ���
        UISoundManager.Instance.SetProgrammaticSelect();
        // �|�[�Y�O�̑I����ۑ�
        lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        _settingsPanel.SetActive(true);
    }

    // <summary>
    // �u�߂�v�{�^������Ăяo����� �ݒ��ʂ����
    // </summary>
    public void HideSettingsPanel()
    {
        _settingsPanel.SetActive(false);
        // SE���Đ�����Ȃ��悤�ɂ���
        UISoundManager.Instance.SetProgrammaticSelect();
        // �|�[�Y�O�̑I���ɖ߂�
        EventSystem.current.SetSelectedGameObject(lastSelectedButton);
    }
}
