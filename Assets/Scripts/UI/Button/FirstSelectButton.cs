using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSelectButton : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelect;

    // Start is called before the first frame update
    void Start()
    {
        // SE���Đ�����Ȃ��悤�ɂ���
        UISoundManager.Instance.SetProgrammaticSelect();
        // �f�t�H���g�̃{�^���ɂ�I������
        EventSystem.current.SetSelectedGameObject(_firstSelect);
    }
}
