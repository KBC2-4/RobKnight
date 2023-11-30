using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNav : MonoBehaviour
{
    //[SerializeField] GameObject canva;

    public GameObject p1;
    public GameObject p2;

    public void OnPushButtonP2()
    {
        p1.SetActive(false);
        p2.SetActive(true);
    }
    public void OnPushButtonP1()
    {
        p1.SetActive(true);
        p2.SetActive(false);
    }

    public void OnPushButtonFin()
    {
        p1.SetActive(false);
        p2.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        OnPushButtonP1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
