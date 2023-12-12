using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CredittController : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 20f;
    [SerializeField] private TMP_Text _creditsText;
    [SerializeField] private GameObject _creditsPanel;
    private Animator _creditsPanelAnimator;

    void Start()
    {
        _creditsPanel.SetActive(true);
        _creditsPanelAnimator = _creditsPanel.GetComponent<Animator>();

    }

    void Update()
    {
        // �e�L�X�g����ɃX�N���[��
        _creditsText.transform.position = new Vector2(_creditsText.transform.position.x, _creditsText.transform.position.y + _scrollSpeed * Time.deltaTime);

        // �A�j���[�V�����̃g���K�[�i����̈ʒu�Ńt�F�[�h�C�����J�n�j
        if (_creditsText.transform.position.y >= 1000)
        {
            // _creditsPanelAnimator.SetTrigger("FadeIn");
        }
    }
}
