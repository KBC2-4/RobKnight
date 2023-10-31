using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyBaseData;
    public EnemyData enemyData;

    private Animator animator;
    public EnemyBehavior behavior;

    private bool finded;
    private bool isAttacking = false;
    private bool playerFound = false;
    public bool isDeath = false;
    public Transform player;
    public float detectionRange = 1f;
    public LayerMask detectionLayer;

    // Start is called before the first frame update
    void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    void Start()
    {
        if (enemyBaseData == null)
        {
            Debug.LogError("EnemyBaseData���Z�b�g����Ă��܂���B");
            return;
        }
        
        enemyData = Instantiate(enemyBaseData);
        animator = GetComponent<Animator>();
        enemyData.hp = enemyData.maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        behavior?.PerformActions(this);
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
        //Destroy(gameObject);
        isDeath = true;
    }

    public void OnMouseDown()
    {
        // �Q�[�����ɃG�l�~�[���N���b�N����ƁA�v���C���[���߈˂���
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            //player.Possess(enemyData);
        }
    }

    //�U���󋵂̎擾
    public bool GetAttacking()
    {
        return isAttacking;
    }

    // �A�j���[�V�����C�x���g����Ăяo�����֐�
    public void PerformAttack()
    {
        isAttacking = true;
        // �����Ńv���C���[�Ƀ_���[�W��^���鏈��������
        //Debug.Log("�G�l�~�[���U��!");
    }
    public void EndAttack()
    {
        isAttacking = false;
        // �����Ńv���C���[�Ƀ_���[�W��^���鏈��������
        //Debug.Log("�G�l�~�[���U���I��!");
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerFound = true;
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerFound = false;
    //    }
    //}

    void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Ray ray = new Ray(transform.position, directionToPlayer);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, detectionRange, detectionLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    // �v���C���[�������������̏���
                    playerFound = true;
                    Debug.Log("�G�l�~�[�F�v���C���[����������I");
                }
                else
                {
                    // �v���C���[��������Ȃ��������̏���
                    playerFound = false;
                }
            }
        }
        else
        {
            // �v���C���[��������Ȃ��������̏���
            playerFound = false;
        }
    }

    public bool IsPlayerFound()
    {
        return playerFound;
    }

}
