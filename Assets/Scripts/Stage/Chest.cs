using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;




public class Chest : MonoBehaviour
{
    private Animator animator;
    public GameObject hintPrefab; // ヒント用のプレファブ
    public string hintMessage;   // ヒントのテキスト
    public List<string> hints; // ヒントのリスト
    private TextMeshProUGUI _hintText;   // ヒントのテキストオブジェクト
    private GameObject _hintInstance; // ヒントのインスタンス

    private Renderer renderer;
    // private Animator _animator;
    private AsyncOperationHandle<GameObject> _handle;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();


        // ヒントUIのインスタンスを作成
        _hintInstance = Instantiate(hintPrefab, transform);
        // 非表示にする
        _hintInstance.SetActive(false);
        _hintText = _hintInstance.GetComponentInChildren<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnTriggerEnter(Collider other)
    {
        //衝突したオブジェクトがプレイヤーの場合
        if (other.gameObject.CompareTag("Player"))
        {
            animator.Play("Open");
           
        }


    }



    void OnTriggerEnter1(Collider other)
    {
        // プレイヤーが近づいたら
        if (other.tag == "Player")
        {
            // UIを表示
            ShowHint(hintMessage);
        }

      
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーが離れたら
        if (other.tag == "Player")
        {
            // UIを非表示
            HideHint();
        }

     
        // _animator.SetTrigger("isHide");
    }

    // ヒントを表示する
    public void ShowHint(string message)
    {
        _hintText.text = message;
        _hintInstance.SetActive(true);
    }



    // ヒントを非表示にする
    public void HideHint()
    {
        _hintInstance.SetActive(false);
    }

    private void OnDestroy()
    {
        // 解放
        Addressables.Release(_handle);
    }
}
 





