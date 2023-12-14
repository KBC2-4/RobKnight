using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleSetting : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    private GameObject lastSelectedButton; // 最後に選択されたボタン

    // Start is called before the first frame update
    void Start()
    {
        // ゲームの時間を初期状態で通常の速度に設定
        Time.timeScale = 1f;

        // 設定画面をアクティブに設定
        _settingsPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // <summary>
    // 「設定」ボタンから呼び出される 設定画面を開く
    // </summary>
    public void ShowSettingsPanel()
    {
        // アクティブの場合は非アクティブにする
        if (_settingsPanel.activeSelf)
        {
            _settingsPanel.SetActive(false);
        }
        // ポーズ前の選択を保存
        lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        _settingsPanel.SetActive(true);
        // SEが再生されないようにする
        UISoundManager.Instance.SetProgrammaticSelect();

    }

    // <summary>
    // 「戻る」ボタンから呼び出される 設定画面を閉じる
    // </summary>
    public void HideSettingsPanel()
    {
        _settingsPanel.SetActive(false);
        // SEが再生されないようにする
        UISoundManager.Instance.SetProgrammaticSelect();
        // ポーズ前の選択に戻す
        EventSystem.current.SetSelectedGameObject(lastSelectedButton);
    }
}
