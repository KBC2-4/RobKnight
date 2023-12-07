using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;




public class Chest : MonoBehaviour
{
    private Animator animator;
    public GameObject hintPrefab; // �q���g�p�̃v���t�@�u
    public string hintMessage;   // �q���g�̃e�L�X�g
    public List<string> hints; // �q���g�̃��X�g
    private TextMeshProUGUI _hintText;   // �q���g�̃e�L�X�g�I�u�W�F�N�g
    private GameObject _hintInstance; // �q���g�̃C���X�^���X

    private Renderer renderer;
    // private Animator _animator;
    private AsyncOperationHandle<GameObject> _handle;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();


        // �q���gUI�̃C���X�^���X���쐬
        _hintInstance = Instantiate(hintPrefab, transform);
        // ��\���ɂ���
        _hintInstance.SetActive(false);
        _hintText = _hintInstance.GetComponentInChildren<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnTriggerEnter(Collider other)
    {
        //�Փ˂����I�u�W�F�N�g���v���C���[�̏ꍇ
        if (other.gameObject.CompareTag("Player"))
        {
            animator.Play("Open");
           
        }


    }



    void OnTriggerEnter1(Collider other)
    {
        // �v���C���[���߂Â�����
        if (other.tag == "Player")
        {
            // UI��\��
            ShowHint(hintMessage);
        }

      
    }

    void OnTriggerExit(Collider other)
    {
        // �v���C���[�����ꂽ��
        if (other.tag == "Player")
        {
            // UI���\��
            HideHint();
        }

     
        // _animator.SetTrigger("isHide");
    }

    // �q���g��\������
    public void ShowHint(string message)
    {
        _hintText.text = message;
        _hintInstance.SetActive(true);
    }



    // �q���g���\���ɂ���
    public void HideHint()
    {
        _hintInstance.SetActive(false);
    }

    private void OnDestroy()
    {
        // ���
        Addressables.Release(_handle);
    }
}
 





