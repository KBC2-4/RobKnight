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

    [SerializeField, Range(0f, 30f), Header("テキストを表示させる秒数")]
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
            _testText.text = "攻撃力が少し上がった!!";
            _backgroundPanel.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);

            // プレイヤーの攻撃力を増加させる
            collider.gameObject.GetComponent<PlayerController>().GetTreasure(10, 10);

            //テキスト
            isOnes = true;
            StartCoroutine(TextDisplayRoutine(textDisplayTime));
        }
    }

    IEnumerator TextDisplayRoutine(float displayTime)
    {
        yield return new WaitForSeconds(displayTime);

        // 2回目のSEを再生
        _audioSorce.clip = _getItem;
        _audioSorce.Play();

        //表示されたテキストを消す
        _canvas.SetActive(false);
        _testText.text = "";
        _backgroundPanel.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
}