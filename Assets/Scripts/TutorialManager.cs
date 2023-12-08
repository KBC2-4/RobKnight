using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DeviceTypeDetector;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialUI; // �`���[�g���A��UI�ւ̎Q��
    public Animator animator; // Animator�ւ̎Q��
    private const string TutorialSeenKey = "TutorialSeen";

    void Start()
    {
        tutorialUI.SetActive(false);

        if (!PlayerPrefs.HasKey(TutorialSeenKey))
        {
            ShowTutorialUI();
        }
    }

    private void ShowTutorialUI()
    {
        tutorialUI.SetActive(true);
        animator.SetTrigger("Show");
        PlayerPrefs.SetInt(TutorialSeenKey, 1);
        PlayerPrefs.Save();
    }
}

