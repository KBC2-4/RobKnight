using System.Collections;
using UnityEngine;
using UnityEngine.UI; // �ǉ�
using TMPro;

public class TreasureScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Image _backgroundPanel; // Image�R���|�[�l���g���g�p���邽�߂̕ϐ�
    private bool isOnes;

    [SerializeField, Range(0f, 30f), Header("�e�L�X�g��\��������b��")]
    private float textDisplayTime; // �e�L�X�g��\��������b��

    void Start()
    {
        animator = GetComponent<Animator>();
        _canvas.SetActive(false);
        isOnes = false;
    }

    // Collider�ɐG�ꂽ���ɌĂ΂�郁�]�b�g
    public void OnTriggerEnter(Collider collider)
    {
        // �v���C���[�ɐڐG
        if (collider.gameObject.tag == "Player" && !isOnes)
        {
            animator.Play("Open");

            _canvas.SetActive(true);
            _testText.text = "�U���͂������オ����!!";

            // �w�i�p�l���̐F��ύX
            _backgroundPanel.color = new Color(0.0f, 0.0f, 0.0f, 0.5f); // RGBA�Ŏw��


            // �v���C���[�̍U���͂𑝉�������
            collider.gameObject.GetComponent<PlayerController>().IncreaseAttackPower();


            isOnes = true;
            StartCoroutine(TextDisplayRoutine(textDisplayTime));
        }
    }

    IEnumerator TextDisplayRoutine(float displayTime)
    {
        yield return new WaitForSeconds(displayTime);
        _canvas.SetActive(false);
        _testText.text = ""; // �e�L�X�g���N���A����i�C�Ӂj
        _backgroundPanel.color = new Color(0.0f, 0.0f, 0.0f, 0.0f); // �p�l���̓����x�����ɖ߂�
    }
}
