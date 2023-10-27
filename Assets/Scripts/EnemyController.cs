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
        Debug.Log("当たってます");
        enemyData.hp -= damage;
        if (enemyData.hp <= 0)
        {
            OnDeath();
        }
    }

    // エネミーが倒されたときの処理
    private void OnDeath()
    {
        animator.SetTrigger("DieTrigger");
        StartCoroutine(DestroyAfterAnimation("Die01", 0));
    }

    private IEnumerator DestroyAfterAnimation(string animationName, int layerIndex)
    {
        // アニメーションの長さを取得
        float animationLength = animator.GetCurrentAnimatorStateInfo(layerIndex).length;

        // アニメーションが完了するのを待つ
        yield return new WaitForSeconds(animationLength);

        // ゲームオブジェクトを破壊
        Destroy(gameObject);
    }

    public void OnMouseDown()
    {
        // ゲーム中にエネミーをクリックすると、プレイヤーが憑依する
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.Possess(enemyData);
        }
    }
}
