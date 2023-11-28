using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionStateManager : MonoBehaviour
{
    public static ActionStateManager Instance;

    public GameObject enemyPossessionUI; // �߈˂����G�l�~�[��UI�v�f�ւ̎Q��
    public Animator uiAnimator;        // Animator�ւ̎Q��

    public Text enemyNameText;         // �G�l�~�[�̖��O��\������Text
    public Image enemyImage;           // �G�l�~�[�摜��\������Image
    public Text enemyDescriptionText;  // ������\������Text

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
        if (info != null)
        {
            enemyNameText.text = info.name;
            enemyImage.sprite = info.image;
            enemyDescriptionText.text = info.description;

            enemyPossessionUI.SetActive(true);
            uiAnimator.SetTrigger("Show");
        }
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
}