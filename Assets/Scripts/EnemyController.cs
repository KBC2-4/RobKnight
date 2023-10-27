using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;
    private Animator animator;

    private bool finded;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyData.hp = enemyData.maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Finded()
    {
        finded = true;
        animator.SetBool("Finded", true);
    }

    public void Damage(int damage)
    {
        Debug.Log("�������Ă܂�");
        enemyData.hp -= damage;
        if (enemyData.hp <= 0)
        {
            OnDeath();
        }
    }

    // �G�l�~�[���|���ꂽ�Ƃ��̏���
    private void OnDeath()
    {
        animator.SetTrigger("DieTrigger");
        StartCoroutine(DestroyAfterAnimation("Die01", 0));
    }

    private IEnumerator DestroyAfterAnimation(string animationName, int layerIndex)
    {
        // �A�j���[�V�����̒������擾
        float animationLength = animator.GetCurrentAnimatorStateInfo(layerIndex).length;

        // �A�j���[�V��������������̂�҂�
        yield return new WaitForSeconds(animationLength);

        // �Q�[���I�u�W�F�N�g��j��
        Destroy(gameObject);
    }

    public void OnMouseDown()
    {
        // �Q�[�����ɃG�l�~�[���N���b�N����ƁA�v���C���[���߈˂���
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.Possess(enemyData);
        }
    }
}
