using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisivility : MonoBehaviour
{

    private GameObject rootobj = null;

    // Start is called before the first frame update
    void Start()
    {
        //�e�I�u�W�F�N�g���擾
        rootobj = transform.root.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }
    // �����蔻�肪�v���C���[�ɐG�ꂽ�Ƃ��̏���
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && rootobj.CompareTag("Enemy"))
        {
            EnemyController enemyController = rootobj.GetComponent<EnemyController>();
            enemyController.Finded();
        }
    }
}
