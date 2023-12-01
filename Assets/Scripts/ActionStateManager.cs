using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ActionStateManager : MonoBehaviour
{
    public static ActionStateManager Instance;

    public GameObject enemyPossessionUI; // �߈˂����G�l�~�[��UI�v�f�ւ̎Q��
    private Animator _uiAnimator;        // Animator�ւ̎Q��

    // public Text enemyNameText;         // �G�l�~�[�̖��O��\������Text
    public Image enemyImage;           // �G�l�~�[�摜��\������Image
    // public Text enemyDescriptionText;  // ������\������Text
    public TextMeshProUGUI enemyNameText;         // �G�l�~�[�̖��O��\������TextMeshProUGUI
    public TextMeshProUGUI enemyDescriptionText;  // ������\������TextMeshProUGUI

    public EnemyInfo[] allEnemyInfo; // ���ׂĂ�EnemyInfo�̔z��

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
        //    Debug.LogError("UI�R���|�[�l���g�����������蓖�Ă��Ă��܂���B");
        //    return;
        //}

        enemyNameText.text = info.displayName;
        enemyImage.sprite = info.image;
        enemyDescriptionText.text = info.description;

        enemyPossessionUI.SetActive(true);
        //_uiAnimator.SetTrigger("Show");
        _uiAnimator.SetFloat(Animator.StringToHash("speed"), 0.3f);
        _uiAnimator.Play("Show");

        // �R���[�`���̋N��
        // StartCoroutine(DelayCoroutine());

        // 5�b��� HideUI ���\�b�h���Ăяo��
        Invoke("HideUI", 5f);

    }

    private void HideUI()
    {
        // �A�j���[�V�������t�Đ�
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

        // 5�b�ԑ҂�
        yield return new WaitForSeconds(5);

        // �A�j���[�V�������t�Đ�
        _uiAnimator.SetFloat(Animator.StringToHash("speed"), -1);
        _uiAnimator.Play("Show", 0, 1f);

        // enemyPossessionUI.SetActive(false);


    }
}