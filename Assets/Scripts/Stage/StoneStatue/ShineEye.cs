using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineEye : MonoBehaviour
{

    [SerializeField] private Stone _stone;

    [SerializeField] Material _color = default;
    private bool _isShine;

    // Start is called before the first frame update
    void Start()
    {
        _isShine = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (_stone.end && !_isShine)
        {
            GetComponent<MeshRenderer>().material = _color;
            _isShine = true;
        }
    }
}
