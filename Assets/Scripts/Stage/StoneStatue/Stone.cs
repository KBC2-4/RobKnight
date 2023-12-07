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

    private AudioSource _audioSorce; // çƒê∂Ç∑ÇÈSE

    public bool end;


    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);

        }
        end = false;

        _audioSorce = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (stonestatueno1.end && stonestatueno2.end && stonestatueno3.end && stonestatueno4.end)
        {
            if (end == false)
            {
                _audioSorce.Play();
                end = true;
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }

            }
        }

    }
}
