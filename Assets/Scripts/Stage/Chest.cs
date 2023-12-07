using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Chest : MonoBehaviour
{
    private Animator animator;
    public GameObject itemPrefab; // �A�C�e���̃v���n�u
    public Text messageText;      // UI�e�L�X�g

    private bool isOpen = false;   // �󔠂��J���ꂽ���ǂ����̃t���O

    private void OnTriggerEnter(Collider other)
    {
        // ���̃I�u�W�F�N�g���g���K�[�R���C�_�[�ɐڐG�������̏���
        if (other.CompareTag("Player") && !isOpen)
        {
            OpenBox();
        }
    }

    // �󔠂��J���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    private void OpenBox()
    {
        StartCoroutine(SpawnItem());
        DisplayMessage("�󔠂��J����܂����I");
        isOpen = true; // �󔠂��J���ꂽ���Ƃ��t���O�Ŏ���
    }


    // �A�C�e���𐶐�����R���[�`��
    private IEnumerator SpawnItem()
    {
        // �����_���Ȉʒu�ɃA�C�e���𐶐�
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 2f, Random.Range(-1f, 1f));
        GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);


        // 3�b��ɃA�C�e�����폜�i�K�X�����j
        yield return new WaitForSeconds(3f);

        Destroy(newItem);
    }

    // ���b�Z�[�W��\�����郁�\�b�h
    private void DisplayMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
    }
}
 





