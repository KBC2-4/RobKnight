using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Stone : MonoBehaviour
{

    public StoneStatueNo1 stonestatueno1;
    public StoneStatueNo2 stonestatueno2;
    public StoneStatueNo3 stonestatueno3;
    public StoneStatueNo4 stonestatueno4;

    public bool end;


    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);

        }
        end = false;
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Debug.Log(stonestatueno1.end);
       
        if (stonestatueno1.end && stonestatueno2.end && stonestatueno3.end && stonestatueno4.end)
        {
            end = true;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }

        }

    }
}
