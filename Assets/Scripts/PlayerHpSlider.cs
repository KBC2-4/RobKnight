using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpSlider : MonoBehaviour
{
    public Slider hpSlider;

    // Start is called before the first frame update
    void Start()
    {
        Slider hpSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update(int hp)
    {
        hpSlider.value = hp;
    }
}
