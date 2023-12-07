using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Stone stone;
    public button Button1;
    public button Button2;

    private Vector3 initialPosition;
    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= initialPosition.y - 6.0f) isOpen = false;
        if(!stone.end) isOpen = false;
        if(!Button1.push_flg) isOpen = false;
        if(!Button2.push_flg) isOpen = false;

        if (isOpen)
        {
            transform.position -= transform.up * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            isOpen = true;
        }
    }
}