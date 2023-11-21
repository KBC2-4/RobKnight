//-------------------------------
//  ボスゴブリン：仲間呼び
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyActions/RushAttack")]
public class RushAttack : EnemyAction
{
    public float RushTime = 1.5f;    //突進の継続時間
    public float RushSpeed = 5;  //突進の初期速度
    public float RushRange = 3;  //突進の攻撃範囲
    public float HitCool = 0.33f;     //突進の当たり判定クールタイム

    private float NowSpeed = 5;  //突進の速度
    private Vector3 direction;  //突進の角度
    private float StartTime;  //突進の開始時間
    private float NowCool;    //現在のクールタイム

    public override void Act(EnemyController controller)
    {
        // 現在再生中のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

        //突進の開始と終了
        if (stateInfo.IsName("StartRush"))  //突進開始時
        {
            StartTime = ActionTime; //突進の開始時間を決定
            NowSpeed = 0;   //突進の速度を初期化

            direction = (controller.player.position - controller.transform.position).normalized;
            // 突進の方向を決める
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            // スムーズに回転させる
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else if (StartTime + RushTime < ActionTime) //突進開始からRushTimeぶんの時間が経過した時
        {
            //突進終了モーションのトリガーをセット
            controller.animator.SetTrigger("RushEndTrigger");
        }

        //突進に伴う移動
        if (stateInfo.IsName("Rush") || stateInfo.IsName("EndRush")) 
        {
            CharacterController Cc = controller.GetComponent<CharacterController>();
            if (Cc != null)
            {
                // エネミーを移動させる
                Vector3 newPosition = direction * (NowSpeed * Time.fixedDeltaTime);
                Cc.Move(newPosition);
            }

            //突進中の行動
            if (stateInfo.IsName("Rush")) 
            {
                //突進開始時、移動速度を徐々に上げる
                NowSpeed += RushSpeed / (1 / Time.fixedDeltaTime * 0.3f);
                if (RushSpeed < NowSpeed) NowSpeed = RushSpeed;

                //プレイヤー間の距離を取る
                float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);
                //プレイヤーの距離が一定以下なら突進ダメージを与える
                if (distanceToPlayer <= RushRange && NowCool <= 0) 
                {
                    controller.player.GetComponent<PlayerController>().Damage(controller.IsGetPower());
                    NowCool = HitCool;
                }
            }
            //突進終了時の行動
            else
            {
                //突進終了時、移動速度を徐々に減らす
                NowSpeed -= RushSpeed / (1 / Time.fixedDeltaTime * 0.5f);
                if (NowSpeed < 0) NowSpeed = 0;

                //トリガーリセット
                controller.animator.ResetTrigger("RushTrigger");
                controller.animator.ResetTrigger("RushEndTrigger");
            }
        }

        //突進クールタイムを減少させる
        NowCool -= Time.fixedDeltaTime;
        if (NowCool < 0) NowCool = 0;

        //アニメーションの状態遷移
        if (stateInfo.IsName("EndRush"))
        {
            if (NowSpeed <= 0)
            {
                //突進モーションが終了後、速度が0以下なら突進アクションを終了する
                IsComplete = true;
                controller.animator.ResetTrigger("RushTrigger");
                controller.animator.ResetTrigger("RushEndTrigger");
                return;
            }
        }
        else if (stateInfo.IsName("Idle") && !IsComplete)
        {
            //突進モーションのトリガーをセット
            controller.animator.SetTrigger("RushTrigger");
        }

    }
}


