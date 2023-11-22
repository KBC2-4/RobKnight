using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shineeye : MonoBehaviour
{

    public Stone stone;

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

        if (stone.end && !isShine)
        {
            GetComponent<MeshRenderer>().material = color;
            isShine = true;
        }
    }
}
