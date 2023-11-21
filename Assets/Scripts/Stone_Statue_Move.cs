using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Statue_Move : MonoBehaviour
{

    public float pushPower;
    public bool push_flg;
    public Vector3 pushDir;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        push_flg = false;
        pushPower = 2.0f;
        rb = this.GetComponent<Rigidbody>();  // rigidbody‚ðŽæ“¾
    }

    // Update is called once per frame
    void Update()
    {
        if (push_flg == true)
        {
            transform.position += -transform.up * Time.deltaTime;
            push_flg = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            pushDir = new Vector3(10 * Time.deltaTime, 0,10 * Time.deltaTime);

            push_flg = true;
        }
    }
}
