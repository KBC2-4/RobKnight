using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TestStoneStatue : MonoBehaviour
{
    /*
     * Še•Ï”‚ÌéŒ¾‚ÌŽd•û
    [Serializable] Int32 a = 3;@@private public‚Ì’†ŠÔ
    private Int32 _b = 4;
    public Int32 c = 5;
    */

    public bool attacked = false; //UŒ‚‚ðH‚ç‚Á‚½‚Ì‚©
    public bool during_rotation = false; //‰ñ“]’†‚È‚Ì‚©

    private float _old_rotaey = 0;


    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (attacked == true && during_rotation == false)
        {
            _old_rotaey = transform.localEulerAngles.y;
            during_rotation = true;
            attacked = false;
        }

        if (during_rotation)
        {
            if (_old_rotaey + 15 >= transform.localEulerAngles.y)
            {
                transform.Rotate(0f, 0f, 6 * Time.deltaTime); //‰ñ“]
            }
            else
            {
                during_rotation = false;
            }
        }
    }
}