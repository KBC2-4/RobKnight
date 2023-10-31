using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float rotationSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalRotation = Input.GetAxis("Mouse X");
        float verticalRotation = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, horizontalRotation * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.left, verticalRotation * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
