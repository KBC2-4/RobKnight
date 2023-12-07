using System;
using System.Collections;
using UnityEngine;
using Color = System.Drawing.Color;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyBaseData;
    public EnemyData enemyData;

    [HideInInspector] public Animator animator;
    public EnemyBehavior behavior;

    private bool _finded;

    private bool _isAttacking = false;  //�U�������ۂ�
    private bool _isHit = false;        //�����蔻�肪�c���Ă��邩�H

    private bool _playerFound = false;
    public bool isDeath = false;
    public Transform player;
    public float detectionRange = 1f;
    public LayerMask detectionLayer;    // Ray�Ō��m���郌�C���[
    //private LayerMask detectionLayer = LayerMask.GetMask("Player", "Wall");
    public float fieldOfViewAngle = 90f; // ����p
    public int rayCount = 10; // ���˂���Ray�̐�
    public GameObject uiPrompt; // �߈˂𑣂�UI
    
    public GameObject lightEffect;
    private ParticleSystem _particleSystem; // �|���ꂽ���ɕ\������p�[�e�B�N��

    // �U���󂯂��ꍇ�̃C�x���g
    public event Action OnDamage;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        
        // �v���C���[��������Ȃ��ꍇ�A�ēx��������
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

        if (behavior != null)
        {
            // �r�w�C�r�A�̏�����(�C�x���g�n���h���̓o�^)
            behavior.Initialize(this);   
        }
        else
        {
            Debug.LogError("EnemyBehavior���Z�b�g����Ă��܂���B");
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDeath)
        {
            if (player != null)
            {
                // �v���C���[�I�u�W�F�N�g����A�N�e�B�u�Ȃ�A�ēx��������
                if (player.root.gameObject.activeSelf == false)
                {
                    player = GameObject.FindWithTag("Player").transform;
                }
                DetectPlayer();
                behavior?.PerformActions(this);
            }
            else
            {
                player = GameObject.FindWithTag("Player").transform;
            }
        }
    }

    // void FixedUpdate()
    // {
    //     if (!isDeath)
    //     {
    //         behavior?.PerformActions(this);
    //     }
    // }

    void OnDestroy()
    {
        if (behavior != null)
        {
            behavior.Cleanup(this);
        };
    }
    
    public void Finded()
    {
        _finded = true;
        animator.SetBool("Finded", true);
    }

    public void Damage(int damage)
    {
        //Debug.Log("�������Ă܂�");
        enemyData.hp -= damage;

        if (enemyData.hp <= 0)
        {
            OnDeath();
        }
        else
        {
            // �_���[�W�C�x���g�𔭉�
            OnDamage?.Invoke();
        }
    }

    // �G�l�~�[���|���ꂽ�Ƃ��̏���
    private void OnDeath()
    {
        animator.SetTrigger("DieTrigger");
        StartCoroutine(DestroyAfterAnimation("Die01", 0));
        transform.rotation = Quaternion.identity;
        lightEffect.SetActive(true);
        // Animator animator = lightEffect.GetComponent<Animator>();
        animator.SetBool("IsWalking", false);
        animator.ResetTrigger("AttackTrigger");
        animator.ResetTrigger("RushTrigger");
        animator.ResetTrigger("CallTrigger");

        animator.SetBool("IsPossession", true);

        EndAttack();
        DisableHit();
        //Animation lightEffectAnimation = lightEffect.GetComponent<Animation>();
        //if (lightEffectAnimation != null)
        //{
        //    lightEffectAnimation.Play();
        //}
        // �p�[�e�B�N����\��
        //_particleSystem = GetComponentInChildren<ParticleSystem>();

        //if (_particleSystem != null)
        //{
        //    _particleSystem.Play();
        //}
        //else
        //{
        //    Debug.Log("�Q�Ƃł��܂���ł����B");
        //}

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        bool isPossession = player.gameObject.GetComponent<PlayerController>().isPossession;

        // �{�X�S�u�����܂��̓v���C���[���߈˂��Ă���G�l���M�[�ȊO��10�b��ɃI�u�W�F�N�g��j������
        if (enemyData.enemyName != "BossGoblin" && isPossession)
        {
            // 10�b��ɃI�u�W�F�N�g��j������B
            Destroy(gameObject, 10.0f);
        }
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
        return _isAttacking;
    }

    // �A�j���[�V�����C�x���g����Ăяo�����֐�
    public void PerformAttack()
    {
        _isAttacking = true;
        // �����Ńv���C���[�Ƀ_���[�W��^���鏈��������
        //Debug.Log("�G�l�~�[���U��!");
    }
    public void EndAttack()
    {
        _isAttacking = false;
        animator.ResetTrigger("AttackTrigger");
        // �����Ńv���C���[�Ƀ_���[�W��^���鏈��������
        //Debug.Log("�G�l�~�[���U���I��!");
    }

    //�U���̓����蔻���L��������
    public void EnableHit()
    {
        _isHit = true;
        // �����Ńv���C���[�Ƀ_���[�W��^���鏈��������
        //Debug.Log("�G�l�~�[���U��!");
    }

    //�U���̓����蔻��𖳌�������
    public void DisableHit()
    {
        _isHit = false;
        // �����Ńv���C���[�Ƀ_���[�W��^���鏈��������
        //Debug.Log("�G�l�~�[���U���I��!");
    }

    void OnTriggerStay(Collider other)
    {
        if (_isHit && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Damage(enemyData.attackPower);
            //playerFound = true;
            _isHit = false;
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
       _playerFound = false;
        
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
                        _playerFound = true;
                        //Debug.Log("�G�l�~�[�F�v���C���[����������I");
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
        return _playerFound;
    }

    //�X�e�[�^�X�擾
    public int IsGetPower() 
    {
        return enemyData.attackPower;
    }

}
