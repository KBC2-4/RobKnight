using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
    public GameObject hintPrefab; // ヒント用のプレファブ
    public string hintMessage;   // ヒントのテキスト
    public List<string> hints; // ヒントのリスト
    private TextMeshProUGUI _hintText;   // ヒントのテキストオブジェクト
    private GameObject _hintInstance; // ヒントのインスタンス
    public Material highlightMaterial;   // ハイライト用のマテリアル
    private Material originalMaterial;   // オリジナルのマテリアル
    private Renderer renderer;


    void Start()
    {
        // ヒントUIのインスタンスを作成
        _hintInstance = Instantiate(hintPrefab, transform);
        // 非表示にする
        _hintInstance.SetActive(false);
        _hintText = _hintInstance.GetComponentInChildren<TextMeshProUGUI>();

        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーが近づいたら
        if (other.tag == "Player")
        {
            // UIを表示
            ShowHint(hintMessage);
        }

        Highlight();
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーが離れたら
        if (other.tag == "Player")
        {
            // UIを非表示
            HideHint();
        }

        RemoveHighlight();
    }

    // ヒントを表示する
    public void ShowHint(string message)
    {
        _hintText.text = message;
        _hintInstance.SetActive(true);
    }

    // ヒントを表示する(複数対応)
    public void ShowHint(int index)
    {
        if (index >= 0 && index < hints.Count)
        {
            _hintText.text = hints[index];
        }
    }

    // ヒントを非表示にする
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