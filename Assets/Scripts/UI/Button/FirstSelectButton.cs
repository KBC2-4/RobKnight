using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSelectButton : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelect;

    // Start is called before the first frame update
    void Start()
    {
        // SEが再生されないようにする
        UISoundManager.Instance.SetProgrammaticSelect();
        // デフォルトのボタンにを選択する
        EventSystem.current.SetSelectedGameObject(_firstSelect);
    }
}
