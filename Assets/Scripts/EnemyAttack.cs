using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 10;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �A�j���[�V�����C�x���g����Ăяo�����֐�
    public void PerformAttack()
    {
        isAttacking = true;
        // �����Ńv���C���[�Ƀ_���[�W��^���鏈��������
        Debug.Log("�G�l�~�[���U��!");
    }

    // �����蔻�肪�v���C���[�ɐG�ꂽ�Ƃ��̏���
    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Damage(attackDamage);
                isAttacking = false;
            }
        }
    }
}
