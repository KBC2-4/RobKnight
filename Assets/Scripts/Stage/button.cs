using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{

    bool push_flg;
    float start_pds_y;

    // Start is called before the first frame update
    void Start()
    {
        push_flg = false;
        start_pds_y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (push_flg == true)
        {
            transform.position  += -transform.up * Time.deltaTime;
            if (start_pds_y-0.1 >= transform.position.y)   { push_flg = false; }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.Player.Body05) { }
        push_flg = true;
    }
}
