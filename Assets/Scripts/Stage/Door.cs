using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool openDoor;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        openDoor = true;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.up * Time.deltaTime;

        if (transform.position.y >= initialPosition.y + 5.0f) openDoor = false;

        if (openDoor)
        {
            transform.position += transform.up * Time.deltaTime;
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.Player.Body05)
    //    {
    //        openDoor = true;
    //    }
    //}
}
