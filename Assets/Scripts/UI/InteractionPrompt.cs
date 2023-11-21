using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    public GameObject uiPrompt;

    void OnTriggerEnter(Collider other)
    {
        // �v���C���[���߂Â�����
        if (other.tag == "Player")
        {
            // UI��\��
            uiPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // �v���C���[�����ꂽ��
        if (other.tag == "Player")
        {
            // UI���\��
            uiPrompt.SetActive(false);
        }
    }
}
