using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDeath() // �G�l�~�[���|���ꂽ�Ƃ��̏���
    {
        // �����Ńv���C���[�ɜ߈˃I�v�V������񎦂��郍�W�b�N��ǉ�
        // ��: UI�̕\����A�߂��̃v���C���[�I�u�W�F�N�g����������Possess���\�b�h���Ăяo��
    }

    private void OnMouseDown()
    {
        // �Q�[�����ɃG�l�~�[���N���b�N����ƁA�v���C���[���߈˂���
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.Possess(enemyData);
        }
    }
}
