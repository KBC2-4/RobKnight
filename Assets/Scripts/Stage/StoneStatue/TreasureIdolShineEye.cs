using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureIdolShineEye : MonoBehaviour
{
    [SerializeField] private button _button1;
    [SerializeField] private button _button2;

    [SerializeField] private Material _color = default;
    private bool _isShine;
    private MeshRenderer _meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _isShine = false;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_button1 != null && _button2 != null)
        {
            if (_button1.push_flg && _button2.push_flg && !_isShine)
            {
                _meshRenderer.material = _color;
                _isShine = true;
            }
        }
    }
}
