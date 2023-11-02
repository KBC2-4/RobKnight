using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{

    bool a;

    // Start is called before the first frame update
    void Start()
    {
        a = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (a = true)
        {
            transform.position += transform.up * Time.deltaTime;
        }
    }
}
