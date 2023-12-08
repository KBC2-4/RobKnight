using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DeviceTypeDetector;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialUI; // チュートリアルUIへの参照
    public Animator animator; // Animatorへの参照
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

