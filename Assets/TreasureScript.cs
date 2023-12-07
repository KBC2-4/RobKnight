using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Image _backgroundPanel;
    private bool isOnes;
    private AudioSource _audioSorce;
    [SerializeField] private AudioClip _getItem;

    public void PlayGetItem()
    {
        _audioSorce.PlayOneShot(_getItem);
    }

    [SerializeField, Range(0f, 30f), Header("�e�L�X�g��\��������b��")]
    private float textDisplayTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        _audioSorce = GetComponent<AudioSource>();

        _canvas.SetActive(false);
        isOnes = false;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && !isOnes)
        {
            animator.Play("Open");
            _audioSorce.Play();
            _canvas.SetActive(true);
            _testText.text = "�U���͂������オ����!!";
            _backgroundPanel.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);

            // �v���C���[�̍U���͂𑝉�������
            collider.gameObject.GetComponent<PlayerController>().GetTreasure(10, 10);

            //�e�L�X�g
            isOnes = true;
            StartCoroutine(TextDisplayRoutine(textDisplayTime));
        }
    }

    IEnumerator TextDisplayRoutine(float displayTime)
    {
        yield return new WaitForSeconds(displayTime);

        // 2��ڂ�SE���Đ�
        _audioSorce.clip = _getItem;
        _audioSorce.Play();

        //�\�����ꂽ�e�L�X�g������
        _canvas.SetActive(false);
        _testText.text = "";
        _backgroundPanel.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
}