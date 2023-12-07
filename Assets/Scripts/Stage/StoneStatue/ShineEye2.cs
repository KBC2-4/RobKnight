using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineEye2 : MonoBehaviour
{
    public button Button1;
    public button Button2;

    [SerializeField] Material color = default;
    Material cubeMaterial;
    private bool isShine;

    // Start is called before the first frame update
    void Start()
    {
        isShine = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Button1.push_flg && Button2.push_flg && !isShine)
        {
            GetComponent<MeshRenderer>().material = color;
            isShine = true;
        }
    }
}
