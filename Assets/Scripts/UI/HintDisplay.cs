using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
    public GameObject hintPrefab; // �q���g�p�̃v���t�@�u
    public string hintMessage;   // �q���g�̃e�L�X�g
    public List<string> hints; // �q���g�̃��X�g
    private TextMeshProUGUI _hintText;   // �q���g�̃e�L�X�g�I�u�W�F�N�g
    private GameObject _hintInstance; // �q���g�̃C���X�^���X
    public Material highlightMaterial;   // �n�C���C�g�p�̃}�e���A��
    private Material originalMaterial;   // �I���W�i���̃}�e���A��
    private Renderer renderer;


    void Start()
    {
        // �q���gUI�̃C���X�^���X���쐬
        _hintInstance = Instantiate(hintPrefab, transform);
        // ��\���ɂ���
        _hintInstance.SetActive(false);
        _hintText = _hintInstance.GetComponentInChildren<TextMeshProUGUI>();

        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
    }

    void OnTriggerEnter(Collider other)
    {
        // �v���C���[���߂Â�����
        if (other.tag == "Player")
        {
            // UI��\��
            ShowHint(hintMessage);
        }

        Highlight();
    }

    void OnTriggerExit(Collider other)
    {
        // �v���C���[�����ꂽ��
        if (other.tag == "Player")
        {
            // UI���\��
            HideHint();
        }

        RemoveHighlight();
    }

    // �q���g��\������
    public void ShowHint(string message)
    {
        _hintText.text = message;
        _hintInstance.SetActive(true);
    }

    // �q���g��\������(�����Ή�)
    public void ShowHint(int index)
    {
        if (index >= 0 && index < hints.Count)
        {
            _hintText.text = hints[index];
        }
    }

    // �q���g���\���ɂ���
    public void HideHint()
    {
        _hintInstance.SetActive(false);
    }

    public void Highlight()
    {
        renderer.material = highlightMaterial;
    }

    public void RemoveHighlight()
    {
        renderer.material = originalMaterial;
    }

}