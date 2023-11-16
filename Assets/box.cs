using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    bool a;
    // Start is called before the first frame update
    void Start()
    {
        button button;
        GameObject obj = GameObject.Find("Cylinder");
        button = obj.GetComponent<button>();
        button.push_flg = true;
    }

    // Update is called once per frame
   /* void Update()
    {
        if (a == true)
        {
            transform.position += transform.up * Time.deltaTime;
            //if (transform.position.y <= 61) { a = false; }
        }
    }*/
}
