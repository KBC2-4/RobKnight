using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class button : MonoBehaviour
{

    public bool push_flg;
    public bool event_flg; //これに中央エリアの石像を光らせるかの確認をする
    float start_pds_y;

    // Start is called before the first frame update
    void Start()
    {
        push_flg = false;
        event_flg = false;
        start_pds_y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (push_flg == true)
        {
            transform.position += -transform.up * Time.deltaTime;
            if (start_pds_y - 0.1 >= transform.position.y) {
                event_flg = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // 衝突したオブジェクトがプレイヤーの場合
        if (other.gameObject.CompareTag("StoneStatue") && !push_flg)
        {
            push_flg = true;

            //スイッチを押した石像を無効化する
            if (other.GetComponent<StoneStatueMove>() != null)
            {
                other.GetComponent<StoneStatueMove>().push_flg = true;
            }
            else
            {
                other.GetComponent<SpinStoneStatueMove>().push_flg = true;
            }
        }
    }
}