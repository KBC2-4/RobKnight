using System.Collections;
using UnityEngine;
using UnityEngine.UI; // 追加
using TMPro;

public class TreasureScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Image _backgroundPanel; // Imageコンポーネントを使用するための変数
    private bool isOnes;

    [SerializeField, Range(0f, 30f), Header("テキストを表示させる秒数")]
    private float textDisplayTime; // テキストを表示させる秒数

    void Start()
    {
        animator = GetComponent<Animator>();
        _canvas.SetActive(false);
        isOnes = false;
    }

    // Colliderに触れた時に呼ばれるメゾット
    public void OnTriggerEnter(Collider collider)
    {
        // プレイヤーに接触
        if (collider.gameObject.tag == "Player" && !isOnes)
        {
            animator.Play("Open");

            _canvas.SetActive(true);
            _testText.text = "攻撃力が少し上がった!!";

            // 背景パネルの色を変更
            _backgroundPanel.color = new Color(0.0f, 0.0f, 0.0f, 0.5f); // RGBAで指定


            // プレイヤーの攻撃力を増加させる
            collider.gameObject.GetComponent<PlayerController>().IncreaseAttackPower();


            isOnes = true;
            StartCoroutine(TextDisplayRoutine(textDisplayTime));
        }
    }

    IEnumerator TextDisplayRoutine(float displayTime)
    {
        yield return new WaitForSeconds(displayTime);
        _canvas.SetActive(false);
        _testText.text = ""; // テキストをクリアする（任意）
        _backgroundPanel.color = new Color(0.0f, 0.0f, 0.0f, 0.0f); // パネルの透明度を元に戻す
    }
}
