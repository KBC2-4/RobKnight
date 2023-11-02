using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Image;
using Color = System.Drawing.Color;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyBaseData;
    public EnemyData enemyData;

    [HideInInspector] public Animator animator;
    public EnemyBehavior behavior;

    private bool finded;
    private bool isAttacking = false;
    private bool playerFound = false;
    public bool isDeath = false;
    public Transform player;
    public float detectionRange = 1f;
    public LayerMask detectionLayer;    // Ray�Ō��m���郌�C���[
    //private LayerMask detectionLayer = LayerMask.GetMask("Player", "Wall");
    public float fieldOfViewAngle = 90f; // ����p
    public int rayCount = 10; // ���˂���Ray�̐�
    
    public GameObject lightEffect;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
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
        enemyData.hp = enemyData.maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDeath)
        {
            DetectPlayer();
            behavior?.PerformActions(this);
        }
    }

    // void FixedUpdate()
    // {
    //     if (!isDeath)
    //     {
    //         behavior?.PerformActions(this);
    //     }
    // }

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
        lightEffect.SetActive(true);
        // Animator animator = lightEffect.GetComponent<Animator>();
        animator.SetBool("IsWalking", false);   
        Animation lightEffectAnimation = lightEffect.GetComponent<Animation>();
        if (lightEffectAnimation != null)
        {
            lightEffectAnimation.Play();
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }

    private IEnumerator DestroyAfterAnimation(string animationName, int layerIndex)
    {
        isDeath = true;

        // �A�j���[�V�����̒������擾
        float animationLength = animator.GetCurrentAnimatorStateInfo(layerIndex).length;

        // �A�j���[�V��������������̂�҂�
        yield return new WaitForSeconds(animationLength);

        // �Q�[���I�u�W�F�N�g��j��
        //Destroy(gameObject);
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
        Debug.Log("�G�l�~�[���U��!");
    }
    public void EndAttack()
    {
        isAttacking = false;
        // �����Ńv���C���[�Ƀ_���[�W��^���鏈��������
        Debug.Log("�G�l�~�[���U���I��!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Player"))
        {
            // if (other.name == "AttackTrigger1")
            // {
            //     
            // }
            other.GetComponent<PlayerController>().Damage(enemyData.attackPower);
            //playerFound = true;
            EndAttack();
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerFound = false;
    //    }
    //}
    
    // �v���C���[�����o���邽�߂̊֐�
    void DetectPlayer()
    {
        // �v���C���[���o�t���O�����Z�b�g
        playerFound = false;
        
        // �G�l�~�[�ƃv���C���[�̊Ԃ̋������v�Z
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // �v���C���[�����o�͈͓��ɂ��邩�`�F�b�N
        if (distanceToPlayer <= detectionRange)
        {
            // �G�l�~�[����v���C���[�ւ̕����x�N�g�����v�Z���A���K��
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            // �G�l�~�[�̑O���x�N�g���ƃv���C���[�ւ̕����x�N�g���̊Ԃ̊p�x���v�Z
            float angleBetween = Vector3.Angle(transform.forward, directionToPlayer);

            // �p�x������̔�����菬�������`�F�b�N
            if (angleBetween < fieldOfViewAngle / 2f)
            {
                
                // �G�l�~�[����v���C���[�ւ̃��C���쐬
                Ray ray = new Ray(transform.position, directionToPlayer);
                RaycastHit hit;
                // ���C��ԐF�ŕ`��i�f�o�b�O�p�j
                Debug.DrawRay(ray.origin, ray.direction * detectionRange, UnityEngine.Color.red);

                // ���C�L���X�g���g�p���ăv���C���[�����o
                if (Physics.Raycast(ray, out hit, detectionRange, detectionLayer))
                {
                    // ���C���v���C���[�Ƀq�b�g�������`�F�b�N
                    if (hit.collider.CompareTag("Player"))
                    {
                        // �v���C���[��������Ȃ��������̏���
                        playerFound = true;
                        Debug.Log("�G�l�~�[�F�v���C���[����������I");
                    }
                    // else�@// Raycast�e�X�g
                    // {
                    //     // �v���C���[��������Ȃ��������̏���
                    //     Debug.Log("�G�l�~�[�FRaycast���q�b�g�������A�v���C���[�ł͂Ȃ��B�q�b�g�����I�u�W�F�N�g�F" + hit.collider.name);
                    //     playerFound = false;
                    // }
                }
            }

            // ���ː����Ray��`��
            for (int i = 0; i < rayCount; i++)
            {
                float angle = fieldOfViewAngle / (rayCount - 1) * i - fieldOfViewAngle / 2f;
                Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;
                Debug.DrawRay(transform.position, rayDirection * detectionRange, UnityEngine.Color.yellow);
            }
        }
    }

    public bool IsPlayerFound()
    {
        return playerFound;
    }

}
