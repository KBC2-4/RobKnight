using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ActionStateManager : MonoBehaviour
{
    public static ActionStateManager Instance;

    public GameObject enemyPossessionUI; // 憑依したエネミーのUI要素への参照
    private Animator _uiAnimator;        // Animatorへの参照

    // public Text enemyNameText;         // エネミーの名前を表示するText
    public Image enemyImage;           // エネミー画像を表示するImage
    // public Text enemyDescriptionText;  // 説明を表示するText
    public TextMeshProUGUI enemyNameText;         // エネミーの名前を表示するTextMeshProUGUI
    public TextMeshProUGUI enemyDescriptionText;  // 説明を表示するTextMeshProUGUI

    public EnemyInfo[] allEnemyInfo; // すべてのEnemyInfoの配列

    private Dictionary<string, bool> firstEnemyPossession = new Dictionary<string, bool>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        LoadActionStates();
        _uiAnimator = GetComponentInChildren<Animator>();
        enemyPossessionUI.SetActive(false);
    }

    //public void PerformAction(string actionName)
    //{
    //    if (!firstEnemyPossession.ContainsKey(actionName))
    //    {
    //        firstEnemyPossession[actionName] = false;
    //    }

    //    if (!firstEnemyPossession[actionName])
    //    {
    //        ShowEnemyPossessionUI(actionName);
    //        firstEnemyPossession[actionName] = true;
    //        SaveActionStates();
    //    }
    //}

    public void RecordEnemyPossession(string enemyType)
    {
        if (!firstEnemyPossession.ContainsKey(enemyType))
        {
            firstEnemyPossession[enemyType] = false;
        }

        if (!firstEnemyPossession[enemyType])
        {
            ShowEnemyPossessionUI(enemyType);
            firstEnemyPossession[enemyType] = true;
            SaveActionStates();
        }
    }

    private void ShowEnemyPossessionUI(string enemyName)
    {
        EnemyInfo info = Array.Find(allEnemyInfo, e => e.name == enemyName);

        if (info == null)
        {
            Debug.LogError($"EnemyInfo not found for enemy name: {enemyName}");
            return;
        }

        //if (enemyNameText == null || enemyImage == null || enemyDescriptionText == null || uiAnimator == null)
        //{
        //    Debug.LogError("UIコンポーネントが正しく割り当てられていません。");
        //    return;
        //}

        enemyNameText.text = info.displayName;
        enemyImage.sprite = info.image;
        enemyDescriptionText.text = info.description;

        enemyPossessionUI.SetActive(true);
        //_uiAnimator.SetTrigger("Show");
        _uiAnimator.SetFloat(Animator.StringToHash("speed"), 0.3f);
        _uiAnimator.Play("Show");

        // コルーチンの起動
        // StartCoroutine(DelayCoroutine());

        // 5秒後に HideUI メソッドを呼び出す
        Invoke("HideUI", 5f);

    }

    private void HideUI()
    {
        // アニメーションを逆再生
        _uiAnimator.SetFloat(Animator.StringToHash("speed"), -1);
        _uiAnimator.Play("Show", 0, 1f);
        enemyPossessionUI.SetActive(false);
    }

    private void SaveActionStates()
    {
        foreach (var action in firstEnemyPossession)
        {
            PlayerPrefs.SetInt(action.Key, action.Value ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadActionStates()
    {
        foreach (var action in firstEnemyPossession)
        {
            if (PlayerPrefs.HasKey(action.Key))
            {
                firstEnemyPossession[action.Key] = PlayerPrefs.GetInt(action.Key) == 1;
            }
        }
    }

    private IEnumerator DelayCoroutine()
    {

        // 5秒間待つ
        yield return new WaitForSeconds(5);

        // アニメーションを逆再生
        _uiAnimator.SetFloat(Animator.StringToHash("speed"), -1);
        _uiAnimator.Play("Show", 0, 1f);

        // enemyPossessionUI.SetActive(false);


    }
}